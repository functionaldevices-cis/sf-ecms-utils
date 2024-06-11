using SF_ECMS_Utils.Helpers;
using SF_ECMS_Utils.Models;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;

namespace SF_ECMS_Utils;
public class SFECMSUtils {

    private Config Config { get; set; }

    private FileOutputUtility FileOutputUtility { get; set; }

    private CMSTitleBuilder TitleBuilder { get; set; }

    public SFECMSUtils(Config config) {

        // PARSE DATA

        this.Config = config;
        this.FileOutputUtility = new(this.Config.PackagedFiles_FolderPath);

        try {

            if (this.Config.Action == "PackageFiles") {      

                // CHECK TO SEE IF THERE IS A TITLES OVERRIDE FILE IN THE ROOT FOLDER

                this.TitleBuilder = this.LoadCMSTitleOverrides(this.Config.SourceFiles_FolderPath);

                // SCAN THE DIRECTORY AND BUILD A LIST OF WHAT FOLDERS AND FILES NEED TO BE PROCESSED

                CMSDirectory directory = this.ScanDirectory(
                    directoryPath: this.Config.SourceFiles_FolderPath,
                    rootPath: this.Config.SourceFiles_FolderPath,
                    isFormatted: false
                );

                // PROCESS THE DIRECTORY

                this.PackageFiles(directory.GetAllFiles());
                this.CreateSummary(directory.GetAllFiles());

                // CREATE ZIP FILE

                if (this.Config.CreateZipFile) {

                    ZipFile.CreateFromDirectory(Path.Combine(this.Config.PackagedFiles_FolderPath, "Packaged Files"), Path.Combine(this.Config.PackagedFiles_FolderPath, "Packaged File.zip"));

                }

            } else {

                // UNZIP IF SPECIFIED

                if (this.Config.PackagedFiles_ZipFilePath != null) {

                    ZipFile.ExtractToDirectory(this.Config.PackagedFiles_ZipFilePath, Path.Combine(this.Config.PackagedFiles_FolderPath, "Packaged Files"));

                }

                // MOVE ALL FILES INTO A NESTED "Packaged Files" FOLDER IF NOT ALREADY DONE

                List<string> subDirectoryPaths = Directory.GetDirectories(this.Config.PackagedFiles_FolderPath).ToList();
                if (subDirectoryPaths.Count == 0 || Path.GetFileName(subDirectoryPaths[0]) != "Packaged Files") {
                    string oldPath = this.Config.PackagedFiles_FolderPath;
                    string newPath = Path.Combine(this.Config.PackagedFiles_FolderPath, "Packaged Files");
                    this.MoveAll(oldPath, newPath);
                }

                // ANALYZE

                CMSDirectory directory = this.ScanDirectory(
                    directoryPath: this.Config.PackagedFiles_FolderPath,
                    rootPath: this.Config.PackagedFiles_FolderPath,
                    isFormatted: true
                );

                this.CreateSummary(directory.GetAllFiles());

            }

        } catch (Exception ex) {

            Console.WriteLine(ex.ToString());

        }

    }

    private CMSTitleBuilder LoadCMSTitleOverrides(string directoryPath) {

        List<CMSTitleOverride> titleOverrides = [];

        string titlesFilePath = Path.Combine(directoryPath, "sfc_titles.csv");

        if (File.Exists(titlesFilePath)) {

            titleOverrides = CSVUtility.UnSerialize(File.ReadAllLines(titlesFilePath).ToList()).Select(
                line => new CMSTitleOverride(
                    cmsPath: line.ContainsKey("CMS Path") ? line["CMS Path"] : "",
                    fileName: line.ContainsKey("File Name") ? line["File Name"] : "",
                    cmsTitle: line.ContainsKey("CMS Title") ? line["CMS Title"] : ""
                )
            ).ToList();

        }

        return new CMSTitleBuilder(titleOverrides);

    }

    private CMSDirectory ScanDirectory(string directoryPath, string rootPath, bool isFormatted) {

        CMSDirectory directory = new(
            directoryName: Path.GetFileName(directoryPath) ?? "",
            directoryPath: directoryPath,
            cmsPath: directoryPath.Replace(rootPath, "").Trim('\\').Replace('\\', '/')
        );

        List<string> subDirectoryPaths;
        List<string> filePaths;
        CMSFile? file;

        // LOAD A LIST OF ALL SUBFOLDERS AND FILES

        if (isFormatted) {

            subDirectoryPaths = Directory.GetDirectories(directory.DirectoryPath).Where(subDirectoryPath => !this.IsDirectoryACMSFile(subDirectoryPath)).ToList();
            filePaths = Directory.GetDirectories(directory.DirectoryPath).Where(subDirectoryPath => this.IsDirectoryACMSFile(subDirectoryPath)).ToList();

        } else {

            subDirectoryPaths = Directory.GetDirectories(directory.DirectoryPath).ToList();
            filePaths = Directory.GetFiles(directory.DirectoryPath, "*.*", SearchOption.TopDirectoryOnly).ToList().Where(filePaths => Path.GetFileName(filePaths) != "sfc_titles.csv").ToList();
            this.TitleBuilder.SetCMSPath(directory.CMSPath);

        }

        // SCAN ALL FILES

        filePaths.ForEach(filePath => {

            if (isFormatted) {
                file = ScanPackagedFileDirectoryPath(
                    filePath: filePath
                );
            } else {
                file = ScanRawFilePath(
                    filePath: filePath,
                    cmsPath: directory.CMSPath
                );
            }

            if (file != null) {
                directory.Files.Add(file);
            }

        });

        // SCAN ALL SUBFOLDERS

        subDirectoryPaths.ForEach(subDirectoryPath => {

            directory.SubDirectories.Add(
                this.ScanDirectory(
                    directoryPath: subDirectoryPath,
                    rootPath: rootPath,
                    isFormatted: isFormatted
                )
            );

        });

        return directory;

    }

    private CMSFile? ScanRawFilePath(string filePath, string cmsPath) {

        CMSFile file = new(
            fileName: Path.GetFileName(filePath),
            cmsTitle: this.TitleBuilder.GetTitle(
                defaultTitle: Path.GetFileNameWithoutExtension(filePath),
                fileName: Path.GetFileName(filePath)
            ),
            filePath: filePath,
            cmsPath: cmsPath,
            cmsMimeType: this.ConvertExtensionToMimeType(Path.GetExtension(filePath))
        );

        return file;

    }

    private CMSFile? ScanPackagedFileDirectoryPath(string filePath) {

        List<string> mediaPaths = Directory.GetFiles(Path.Combine(filePath, "_media")).ToList();

        if (mediaPaths.Count > 0) {

            string mediaPath = mediaPaths[0];
            string contentJSONPath = Path.Combine(filePath, "content.json");
            string metaJSONPath = Path.Combine(filePath, "_meta.json");

            if (File.Exists(contentJSONPath) && File.Exists(metaJSONPath)) {

                string contentJSONFileContent = File.ReadAllText(contentJSONPath);
                JSON_Content contentJSON = JsonSerializer.Deserialize(contentJSONFileContent, JSON_ContentContext.Default.JSON_Content) ?? throw new Exception("Error, unable to parse a content file.");

                string metaJSONFileContent = File.ReadAllText(metaJSONPath);
                JSON_Meta metaJSON = JsonSerializer.Deserialize(metaJSONFileContent, JSON_MetaContext.Default.JSON_Meta) ?? throw new Exception("Error, unable to parse a meta file.");

                return new CMSFile(
                    fileName: Path.GetFileName(mediaPath),
                    filePath: mediaPath,
                    cmsTitle: contentJSON.title,
                    cmsMimeType: contentJSON.contentBody.sfdc_cms_media.source.mimeType,
                    cmsPath: metaJSON.path,
                    CMSContentKey: metaJSON.contentKey
                );

            }

        }

        return null;

    }

    private void PackageFiles(List<CMSFile> files) {

        files.ForEach(file => {

            // CREATE PATHS

            string outputFileWrapperFolderPath = Path.Combine(this.Config.PackagedFiles_FolderPath, "Packaged Files", file.CMSPath, file.FileName);

            // CREATE NEW FOLDERS

            if (!Directory.Exists(outputFileWrapperFolderPath)) {
                Directory.CreateDirectory(outputFileWrapperFolderPath);
                Directory.CreateDirectory(Path.Combine(outputFileWrapperFolderPath, "_media"));
            }

            // COPY THE FILE INTO THE NEW LOCATION

            File.Copy(file.FilePath, Path.Combine(outputFileWrapperFolderPath, "_media", file.FileName), true);

            // CREATE THE JSON FILES

            this.FileOutputUtility.CreateFile(
                filePathWithinRoot: Path.Combine(outputFileWrapperFolderPath, "content.json"),
                fileContents: JsonSerializer.Serialize(file.CMSContentJSON, JSON_ContentContext.Default.JSON_Content)
            );

            this.FileOutputUtility.CreateFile(
                filePathWithinRoot: Path.Combine(outputFileWrapperFolderPath, "_meta.json"),
                fileContents: JsonSerializer.Serialize(file.CMSMetaJSON, JSON_MetaContext.Default.JSON_Meta)
            );

        });

    }

    private void CreateSummary(List<CMSFile> files) {

        // CREATE A LIST OF TEXT LINES

        List<string> csvFileLines = CSVUtility.Serialize(
            files.Select(file => file.AnalysisValues).ToList()
        );

        // WRITE THE LINES TO A FILE

        this.FileOutputUtility.CreateFile(
            filePathWithinRoot: "Files Summary.csv",
            fileLines: csvFileLines
        );

    }

    private string ConvertExtensionToMimeType(string extension) {

        return extension switch {
            ".aac" => "audio/aac",
            ".abw" => "application/x-abiword",
            ".apng" => "image/apng",
            ".arc" => "application/x-freearc",
            ".avif" => "image/avif",
            ".avi" => "video/x-msvideo",
            ".azw" => "application/vnd.amazon.ebook",
            ".bin" => "application/octet-stream",
            ".bmp" => "image/bmp",
            ".bz" => "application/x-bzip",
            ".bz2" => "application/x-bzip2",
            ".cda" => "application/x-cdf",
            ".csh" => "application/x-csh",
            ".css" => "text/css",
            ".csv" => "text/csv",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".eot" => "application/vnd.ms-fontobject",
            ".epub" => "application/epub+zip",
            ".gz" => "application/gzip",
            ".gif" => "image/gif",
            ".htm" => "text/html",
            ".html" => "text/html",
            ".ico" => "image/vnd.microsoft.icon",
            ".ics" => "text/calendar",
            ".jar" => "application/java-archive",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".js" => "text/javascript",
            ".json" => "application/json",
            ".jsonld" => "application/ld+json",
            ".mid" => "audio/midi, audio/x-midi",
            ".midi" => "audio/midi, audio/x-midi",
            ".mjs" => "text/javascript",
            ".mp3" => "audio/mpeg",
            ".mp4" => "video/mp4",
            ".mpeg" => "video/mpeg",
            ".mpkg" => "application/vnd.apple.installer+xml",
            ".odp" => "application/vnd.oasis.opendocument.presentation",
            ".ods" => "application/vnd.oasis.opendocument.spreadsheet",
            ".odt" => "application/vnd.oasis.opendocument.text",
            ".oga" => "audio/ogg",
            ".ogv" => "video/ogg",
            ".ogx" => "application/ogg",
            ".opus" => "audio/opus",
            ".otf" => "font/otf",
            ".png" => "image/png",
            ".pdf" => "application/pdf",
            ".php" => "application/x-httpd-php",
            ".ppt" => "application/vnd.ms-powerpoint",
            ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            ".rar" => "application/vnd.rar",
            ".rtf" => "application/rtf",
            ".sh" => "application/x-sh",
            ".svg" => "image/svg+xml",
            ".tar" => "application/x-tar",
            ".tif" => "image/tiff",
            ".tiff" => "image/tiff",
            ".ts" => "video/mp2t",
            ".ttf" => "font/ttf",
            ".txt" => "text/plain",
            ".vsd" => "application/vnd.visio",
            ".wav" => "audio/wav",
            ".weba" => "audio/webm",
            ".webm" => "video/webm",
            ".webp" => "image/webp",
            ".woff" => "font/woff",
            ".woff2" => "font/woff2",
            ".xhtml" => "application/xhtml+xml",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".xml" => "application/xml",
            ".xul" => "application/vnd.mozilla.xul+xml",
            ".zip" => "application/zip",
            ".3gp" => "audio/3gpp",
            ".3g2" => "audio/3gpp2",
            ".7z" => "application/x-7z-compressed",
            _ => "application/octet-stream"
        };

    }

    private bool IsDirectoryACMSFile(string directoryPath) {

        List<string> subDirectoryPaths = Directory.GetDirectories(directoryPath).ToList();

        if (subDirectoryPaths.Count == 1 && Path.GetFileName(subDirectoryPaths[0]) == "_media") {

            return true;

        } else {

            return false;
        }

    }

    private void MoveAll(string oldPath, string newPath) {

        List<string> subDirectoryPaths = Directory.GetDirectories(oldPath).ToList();
        List<string> filePaths = Directory.GetFiles(oldPath, "*.*", SearchOption.TopDirectoryOnly).ToList();

        if (!Directory.Exists(newPath)) {
            Directory.CreateDirectory(newPath);
        }

        // MOVE ALL FILES

        filePaths.ForEach(filePath => {

            string fileName = Path.GetFileName(filePath);

            File.Move(filePath, Path.Combine(newPath,filePath));

        });

        // MOVE ALL SUBFOLDERS

        subDirectoryPaths.ForEach(subDirectoryPath => {

            string folderName = Path.GetFileName(subDirectoryPath);

            Directory.Move(subDirectoryPath, Path.Combine(newPath, folderName));

        });

    }

}
