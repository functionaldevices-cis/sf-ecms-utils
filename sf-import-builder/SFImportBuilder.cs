using SF_Import_Builder.Helpers;
using SF_Import_Builder.Models;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;

namespace SF_Import_Builder; 
public class SFImportBuilder {

    private Config Config { get; set; }

    private FileOutputUtility FileOutputUtility { get; set; }

    private List<CMSTitleOverride> TitleOverrides { get; set; } = new();

    public SFImportBuilder(Config config) {

        // PARSE DATA

        this.Config = config;
        this.FileOutputUtility = new(this.Config.OutputFolderPath);

        try {

            // VALIDATE DATA

            if (!Directory.Exists(this.Config.SourceFolderPath)) {

                throw new Exception("Error, the source folder that is specified in the config file doesn't seem to exist.");

            }

            // CHECK TO SEE IF THERE IS A TITLES OVERRIDE FILE IN THE ROOT FOLDER

            this.TitleOverrides = this.LoadCMSTitleOverrides(this.Config.SourceFolderPath);

            // SCAN THE DIRECTORY AND BUILD A LIST OF WHAT FOLDERS AND FILES NEED TO BE PROCESSED

            CMSDirectory directory = this.ScanUnformattedDirectory(
                directoryPath: this.Config.SourceFolderPath,
                rootPath: this.Config.SourceFolderPath
            );

            // PROCESS THE DIRECTORY

            this.ProcessDirectory(directory);

            // CREATE ZIP FILE

            if (this.Config.CreateZipPackage) {

                ZipFile.CreateFromDirectory(Path.Combine(this.Config.OutputFolderPath, "ImportPackage"), Path.Combine(this.Config.OutputFolderPath, "ImportPackage.zip"));

            }

            if (this.Config.DeleteOutputFolder) {

                Directory.Delete(Path.Combine(this.Config.OutputFolderPath, "ImportPackage"), true);

            }

        } catch (Exception ex) {

            Console.WriteLine(ex.ToString());

        }

    }

    private List<CMSTitleOverride> LoadCMSTitleOverrides(string directoryPath) {

        List<CMSTitleOverride> titleOverrides = new();

        string titlesFilePath = Path.Combine(directoryPath, "sfc_titles.csv");

        if (File.Exists(titlesFilePath)) {

            titleOverrides = CSVUtility.UnSerialize(File.ReadAllLines(titlesFilePath)).Select(
                line => new CMSTitleOverride(
                    cmsPath: line.ContainsKey("CMS Path") ? line["CMS Path"] : "",
                    fileName: line.ContainsKey("File Name") ? line["File Name"] : "",
                    cmsTitle: line.ContainsKey("CMS Title") ? line["CMS Title"] : ""
                )
            ).ToList();

        }

        return titleOverrides;

    }

    private CMSDirectory ScanUnformattedDirectory(string directoryPath, string rootPath) {

        CMSDirectory directory = new(
            name: Path.GetFileName(directoryPath) ?? "",
            fullPath: directoryPath,
            pathWithinRoot: directoryPath.Replace(rootPath, "").Trim('\\')
        );

        // LOAD A LIST OF ALL SUBFOLDERS AND FILES

        Console.WriteLine($"Scanning folder '{Path.Combine(Path.GetFileName(rootPath), directory.PathWithinRoot)}'.");

        List<string> subDirectoryPaths = Directory.GetDirectories(directory.FullPath).ToList();
        List<string> filePaths = Directory.GetFiles(directory.FullPath, "*.*", SearchOption.TopDirectoryOnly).ToList();

        // SCAN ALL FILES

        string? defaultCMSTitleForDirectory = null;
        List<CMSTitleOverride> cmsTitlesForDirectory = this.TitleOverrides.Where(overRide => overRide.CMSPath == directory.PathWithinRoot).ToList();
        List<CMSTitleOverride> matchingCMSTitles = cmsTitlesForDirectory.Where(overRide => overRide.FileName == "*").ToList();
        if (matchingCMSTitles.Count > 0) {
            defaultCMSTitleForDirectory = matchingCMSTitles[0].CMSTitle;
        }

        filePaths.ForEach(filePath => {

            if (filePath != Path.Combine(directoryPath, "sfc_titles.csv")) {

                CMSFile file = this.ScanFile(
                    filePath: filePath,
                    cmsPath: directory.PathWithinRoot
                );

                matchingCMSTitles = cmsTitlesForDirectory.Where(overRide => overRide.FileName == file.File_Name).ToList();

                if (matchingCMSTitles.Count > 0) {
                    file.Content_Title = matchingCMSTitles[0].CMSTitle;
                } else if (defaultCMSTitleForDirectory != null) {
                    file.Content_Title = defaultCMSTitleForDirectory;
                }

                directory.Files.Add(file);

            }

        });

        // SCAN ALL SUBFOLDERS

        subDirectoryPaths.ForEach(subDirectoryPath => {

            directory.SubDirectories.Add(
                this.ScanUnformattedDirectory(
                    directoryPath: subDirectoryPath,
                    rootPath: rootPath
                )
            );

        });

        return directory;

    }

    private CMSFile ScanFile(string filePath, string cmsPath, string? cmsTitle = null) {

        CMSFile file = new(
            file_Name: Path.GetFileName(filePath),
            content_Title: cmsTitle ?? Path.GetFileNameWithoutExtension(filePath),
            file_Path: filePath,
            meta_Path: cmsPath,
            content_MimeType: this.ConvertExtensionToMimeType(Path.GetExtension(filePath))
        );

        return file;

    }

    private void ProcessDirectory(CMSDirectory directory) {

        // PROCESS THE FILES THAT ARE DIRECTLY IN THIS DIRECTORY

        for (int i = 0; i < directory.Files.Count; i++) {
            this.ProcessFile(directory.Files[i]);
        }

        // LOOP THROUGH THE SUBDIRECTORIES AND PROCESS EACH ONE

        directory.SubDirectories.ForEach(subDirectory => {
            this.ProcessDirectory(subDirectory);
        });

    }

    private void ProcessFile(CMSFile file) {

        // CREATE PATHS

        string outputFileWrapperFolderPath = Path.Combine(this.Config.OutputFolderPath, "ImportPackage", file.Meta_Path, file.File_Name);

        // CREATE NEW FOLDERS

        if (!Directory.Exists(outputFileWrapperFolderPath)) {
            Directory.CreateDirectory(outputFileWrapperFolderPath);
            Directory.CreateDirectory(Path.Combine(outputFileWrapperFolderPath, "_media"));
        }

        // COPY THE FILE INTO THE NEW LOCATION

        File.Copy(file.File_Path, Path.Combine(outputFileWrapperFolderPath, "_media", file.File_Name), true);

        // CREATE THE JSON FILES

        this.FileOutputUtility.CreateFile(
            filePathWithinRoot: Path.Combine(outputFileWrapperFolderPath, "content.json"),
            fileContents: JsonSerializer.Serialize(file.Content_JSON, JSON_ContentContext.Default.JSON_Content)
        );

        this.FileOutputUtility.CreateFile(
            filePathWithinRoot: Path.Combine(outputFileWrapperFolderPath, "_meta.json"),
            fileContents: JsonSerializer.Serialize(file.Meta_JSON, JSON_MetaContext.Default.JSON_Meta)
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
            ".htm, .html" => "text/html",
            ".ico" => "image/vnd.microsoft.icon",
            ".ics" => "text/calendar",
            ".jar" => "application/java-archive",
            ".jpeg, .jpg" => "image/jpeg",
            ".js" => "text/javascript",
            ".json" => "application/json",
            ".jsonld" => "application/ld+json",
            ".mid, .midi" => "audio/midi, audio/x-midi",
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
            ".tif, .tiff" => "image/tiff",
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

}
