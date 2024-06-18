using CsvHelper.Configuration;
using CsvHelper;
using SF_ECMS_Utils.Helpers;
using SF_ECMS_Utils.Models;
using System.Diagnostics;
using System.Globalization;
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

    private Dictionary<string, CMSFileType> CMSFileTypes { get; set; }

    public SFECMSUtils(Config config) {

        // PARSE DATA

        this.Config = config;
        this.FileOutputUtility = new(this.Config.PackagedFiles_FolderPath);
        this.TitleBuilder = new(this.LoadCMSTitleOverrides(this.Config.SourceFiles_FolderPath));
        this.CMSFileTypes = this.SetupCMSFileTypes();

        try {

            if (this.Config.Action == "PackageFiles") {      

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

    private List<CSV_CMSTitleOverride> LoadCMSTitleOverrides(string directoryPath) {

        try {

            if (File.Exists(Path.Combine(directoryPath, "sfc_titles.csv"))) {
                StreamReader reader = new(Path.Combine(directoryPath, "sfc_titles.csv"));
                CsvReader csv = new(reader, CultureInfo.InvariantCulture);
                return csv.GetRecords<CSV_CMSTitleOverride>().ToList();
            }

        } catch (Exception ex) {

        }

        return [];

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


        CMSFileType fileType = this.GetCMSFileType(Path.GetExtension(filePath));

        CMSFile file = new(
            fileName: Path.GetFileName(filePath),
            cmsType: fileType.CMSType,
            cmsTitle: this.TitleBuilder.GetTitle(
                defaultTitle: Path.GetFileNameWithoutExtension(filePath),
                fileName: Path.GetFileName(filePath)
            ),
            filePath: filePath,
            cmsPath: cmsPath,
            cmsMimeType: fileType.MimeType
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
                    cmsType: contentJSON.type,
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

        // WRITE THE LINES TO A FILE

        this.FileOutputUtility.CreateCSVFile(
            filePathWithinRoot: "Files Summary.csv",
            records: files.Select(file => file.SummarizedValues).ToList()
        );

    }
    
    private CMSFileType GetCMSFileType(string extension) {

        if ( this.CMSFileTypes.ContainsKey(extension)) {

            return this.CMSFileTypes[extension];

        } else {

            return this.CMSFileTypes[""];

        }

    }

    private Dictionary<string, CMSFileType> SetupCMSFileTypes() {

        List<CMSFileType> fileTypes = [
            new(".aac", "sfdc_cms__document", "audio/aac" ),
            new(".abw", "sfdc_cms__document", "application/x-abiword" ),
            new(".apng", "sfdc_cms__document", "image/apng" ),
            new(".arc", "sfdc_cms__document", "application/x-freearc" ),
            new(".avif", "sfdc_cms__document", "image/avif" ),
            new(".avi", "sfdc_cms__document", "video/x-msvideo" ),
            new(".azw", "sfdc_cms__document", "application/vnd.amazon.ebook" ),
            new(".bin", "sfdc_cms__document", "application/octet-stream" ),
            new(".bmp", "sfdc_cms__document", "image/bmp" ),
            new(".bz", "sfdc_cms__document", "application/x-bzip" ),
            new(".bz2", "sfdc_cms__document", "application/x-bzip2" ),
            new(".cda", "sfdc_cms__document", "application/x-cdf" ),
            new(".csh", "sfdc_cms__document", "application/x-csh" ),
            new(".css", "sfdc_cms__document", "text/css" ),
            new(".csv", "sfdc_cms__document", "text/csv" ),
            new(".doc", "sfdc_cms__document", "application/msword" ),
            new(".docx", "sfdc_cms__document", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" ),
            new(".dwg", "sfdc_cms__document", "image/vnd.dwg" ),
            new(".eot", "sfdc_cms__document", "application/vnd.ms-fontobject" ),
            new(".epub", "sfdc_cms__document", "application/epub+zip" ),
            new(".gz", "sfdc_cms__document", "application/gzip" ),
            new(".gif", "sfdc_cms__document", "image/gif" ),
            new(".htm", "sfdc_cms__document", "text/html" ),
            new(".html", "sfdc_cms__document", "text/html" ),
            new(".ico", "sfdc_cms__document", "image/vnd.microsoft.icon" ),
            new(".ics", "sfdc_cms__document", "text/calendar" ),
            new(".jar", "sfdc_cms__document", "application/java-archive" ),
            new(".jpg", "sfdc_cms__document", "image/jpeg" ),
            new(".jpeg", "sfdc_cms__document", "image/jpeg" ),
            new(".js", "sfdc_cms__document", "text/javascript" ),
            new(".json", "sfdc_cms__document", "application/json" ),
            new(".jsonld", "sfdc_cms__document", "application/ld+json" ),
            new(".mid", "sfdc_cms__document", "audio/midi, audio/x-midi" ),
            new(".midi", "sfdc_cms__document", "audio/midi, audio/x-midi" ),
            new(".mjs", "sfdc_cms__document", "text/javascript" ),
            new(".mp3", "sfdc_cms__document", "audio/mpeg" ),
            new(".mp4", "sfdc_cms__document", "video/mp4" ),
            new(".mpeg", "sfdc_cms__document", "video/mpeg" ),
            new(".mpkg", "sfdc_cms__document", "application/vnd.apple.installer+xml" ),
            new(".odp", "sfdc_cms__document", "application/vnd.oasis.opendocument.presentation" ),
            new(".ods", "sfdc_cms__document", "application/vnd.oasis.opendocument.spreadsheet" ),
            new(".odt", "sfdc_cms__document", "application/vnd.oasis.opendocument.text" ),
            new(".oga", "sfdc_cms__document", "audio/ogg" ),
            new(".ogv", "sfdc_cms__document", "video/ogg" ),
            new(".ogx", "sfdc_cms__document", "application/ogg" ),
            new(".opus", "sfdc_cms__document", "audio/opus" ),
            new(".otf", "sfdc_cms__document", "font/otf" ),
            new(".png", "sfdc_cms__document", "image/png" ),
            new(".pdf", "sfdc_cms__document", "application/pdf" ),
            new(".php", "sfdc_cms__document", "application/x-httpd-php" ),
            new(".ppt", "sfdc_cms__document", "application/vnd.ms-powerpoint" ),
            new(".pptx", "sfdc_cms__document", "application/vnd.openxmlformats-officedocument.presentationml.presentation" ),
            new(".rar", "sfdc_cms__document", "application/vnd.rar" ),
            new(".rtf", "sfdc_cms__document", "application/rtf" ),
            new(".sh", "sfdc_cms__document", "application/x-sh" ),
            new(".svg", "sfdc_cms__document", "image/svg+xml" ),
            new(".tar", "sfdc_cms__document", "application/x-tar" ),
            new(".tif", "sfdc_cms__document", "image/tiff" ),
            new(".tiff", "sfdc_cms__document", "image/tiff" ),
            new(".ts", "sfdc_cms__document", "video/mp2t" ),
            new(".ttf", "sfdc_cms__document", "font/ttf" ),
            new(".txt", "sfdc_cms__document", "text/plain" ),
            new(".vsd", "sfdc_cms__document", "application/vnd.visio" ),
            new(".wav", "sfdc_cms__document", "audio/wav" ),
            new(".weba", "sfdc_cms__document", "audio/webm" ),
            new(".webm", "sfdc_cms__document", "video/webm" ),
            new(".webp", "sfdc_cms__document", "image/webp" ),
            new(".woff", "sfdc_cms__document", "font/woff" ),
            new(".woff2", "sfdc_cms__document", "font/woff2" ),
            new(".xhtml", "sfdc_cms__document", "application/xhtml+xml" ),
            new(".xls", "sfdc_cms__document", "application/vnd.ms-excel" ),
            new(".xlsx", "sfdc_cms__document", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ),
            new(".xml", "sfdc_cms__document", "application/xml" ),
            new(".xul", "sfdc_cms__document", "application/vnd.mozilla.xul+xml" ),
            new(".zip", "sfdc_cms__document", "application/zip" ),
            new(".3gp", "sfdc_cms__document", "audio/3gpp" ),
            new(".3g2", "sfdc_cms__document", "audio/3gpp2" ),
            new(".7z", "sfdc_cms__document", "application/x-7z-compressed" ),
            new("", "sfdc_cms__document", "application/octet-stream" )
        ];

        return fileTypes.ToDictionary(
            t => t.Extension,
            t => t
        );

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
