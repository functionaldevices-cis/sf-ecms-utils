using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Import_Builder.Helpers; 

public class FileOutputUtility {

    public string RootFolder { get; set; }

    public FileOutputUtility(string rootFolder) {

        this.RootFolder = rootFolder;

    }

    public void CreateFile(string filePathWithinRoot, string fileContents) {

        if (filePathWithinRoot.Contains('\\')) {

            string nestedFolderPath;
            string[] fileNameParts;

            fileNameParts = filePathWithinRoot.Split('\\');
            fileNameParts = fileNameParts.Take(fileNameParts.Length - 1).ToArray();
            nestedFolderPath = string.Join('\\', fileNameParts);

            Directory.CreateDirectory(Path.Combine(this.RootFolder, nestedFolderPath));

        }

        StreamWriter sw = new(Path.Combine(this.RootFolder, filePathWithinRoot), false);
        sw.Write(fileContents);
        sw.Close();


    }

}
