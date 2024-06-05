using SF_Import_Builder.Helpers;
using SF_Import_Builder.Models;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SF_Import_Builder; 
public class SFImportBuilder {

    private Config Config { get; set; }

    private FileOutputUtility FileOutputUtility { get; set; }

    public SFImportBuilder(Config config) {

        // PARSE DATA

        this.Config = config;
        this.FileOutputUtility = new(this.Config.OutputFolderPath);

        try {

            // VALIDATE DATA

            if (!Directory.Exists(this.Config.SourceFolderPath)) {

                throw new Exception("Error, the source folder that is specified in the config file doesn't seem to exist.");

            }

            // SCAN THE DIRECTORY AND BUILD A LIST OF WHAT FOLDERS AND FILES NEED TO BE PROCESSED

            ImporteeDirectory directory = this.ScanDirectory(
                directoryPath: this.Config.SourceFolderPath,
                rootPath: this.Config.SourceFolderPath
            );

            // PROCESS THE DIRECTORY

            this.ProcessDirectory(directory);

        } catch (Exception ex) {

            Console.WriteLine(ex.ToString());

        }

    }

    private ImporteeDirectory ScanDirectory(string directoryPath, string rootPath) {

        ImporteeDirectory directory = new(
            name: Path.GetFileName(directoryPath) ?? "",
            fullPath: directoryPath,
            pathWithinRoot: directoryPath.Replace(rootPath, "").Trim('\\')
        );

        // LOAD A LIST OF ALL SUBFOLDERS AND FILES

        Console.WriteLine($"Scanning folder '{Path.Combine(Path.GetFileName(rootPath), directory.PathWithinRoot)}'.");

        List<string> subDirectoryPaths = Directory.GetDirectories(directory.FullPath).ToList();
        List<string> filePaths = Directory.GetFiles(directory.FullPath, "*.*", SearchOption.TopDirectoryOnly).ToList();

        // SCAN ALL FILES

        filePaths.ForEach(filePath => {

            if (File.Exists(filePath)) {

                directory.Files.Add(
                    this.ScanFile(
                        filePath: filePath,
                        rootPath: rootPath
                    )
                 );

            }

        });

        // SCAN ALL SUBFOLDERS

        subDirectoryPaths.ForEach(subDirectoryPath => {

            if (Directory.Exists(subDirectoryPath)) {

                directory.SubDirectories.Add(
                    this.ScanDirectory(
                        directoryPath: subDirectoryPath,
                        rootPath: rootPath
                    )
                );

            }

        });

        return directory;

    }

    private ImporteeFile ScanFile(string filePath, string rootPath) {

        ImporteeFile file = new(
            name: Path.GetFileName(filePath),
            nameWithoutExtension: Path.GetFileNameWithoutExtension(filePath),
            fullPath: filePath,
            pathWithinRoot: filePath.Replace(rootPath, "").Replace(Path.GetFileName(filePath), "").Trim('\\'),
            extension: Path.GetExtension(filePath)
        );

        return file;

    }

    private void ProcessDirectory(ImporteeDirectory directory) {

        // PROCESS THE FILES THAT ARE DIRECTLY IN THIS DIRECTORY

        for (int i = 0; i < directory.Files.Count; i++) {

            Console.Clear();
            Console.WriteLine($"Processing files directly in '{directory.FullPath}'.");
            Console.WriteLine($"Processing file {i+1} out of {directory.Files.Count} total.");

            this.ProcessFile(directory.Files[i]);

        }

        // LOOP THROUGH THE SUBDIRECTORIES AND PROCESS EACH ONE

        directory.SubDirectories.ForEach(subDirectory => {
            this.ProcessDirectory(subDirectory);
        });

    }

    private void ProcessFile(ImporteeFile file) {

        // CREATE PATHS

        string outputFileWrapperFolderPath = Path.Combine(this.Config.OutputFolderPath, file.PathWithinRoot, file.Name);

        // CREATE NEW FOLDERS

        if (!Directory.Exists(outputFileWrapperFolderPath)) {
            Directory.CreateDirectory(outputFileWrapperFolderPath);
            Directory.CreateDirectory(Path.Combine(outputFileWrapperFolderPath, "_media"));
        }

        // COPY THE FILE INTO THE NEW LOCATION

        File.Copy(file.FullPath, Path.Combine(outputFileWrapperFolderPath, "_media", file.Name), true);

        // CREATE THE JSON FILES

        JsonSerializerOptions serializer = new() {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true,
        };

        this.FileOutputUtility.CreateFile(
            filePathWithinRoot: Path.Combine(outputFileWrapperFolderPath, "content.json"),
            fileContents: JsonSerializer.Serialize(file.ContentJSON, serializer)
        );

        this.FileOutputUtility.CreateFile(
            filePathWithinRoot: Path.Combine(outputFileWrapperFolderPath, "_meta.json"),
            fileContents: JsonSerializer.Serialize(file.MetaJSON, serializer)
        );

    }

}
