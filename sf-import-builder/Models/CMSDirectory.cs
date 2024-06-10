using System.Collections.Generic;
using System.IO;

namespace SF_ECMS_Utils.Models; 
public class CMSDirectory {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public string Name { get; init; }
    public string FullPath { get; init; }
    public string PathWithinRoot { get; init; }
    public List<CMSDirectory> SubDirectories { get; init; }
    public List<CMSFile> Files { get; init; }



    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public CMSDirectory(string name, string fullPath, string pathWithinRoot) {

        this.Name = name;
        this.FullPath = fullPath;
        this.PathWithinRoot = pathWithinRoot;
        this.SubDirectories = [];
        this.Files = [];

    }

    /***********************************************************************************************************/
    /************************************************* METHODS *************************************************/
    /***********************************************************************************************************/

    public List<CMSFile> GetAllFiles () {

        // PROCESS THE FILES THAT ARE DIRECTLY IN THIS DIRECTORY

        List<CMSFile> files = [];
        
        files.AddRange(this.Files);

        // LOOP THROUGH THE SUBDIRECTORIES AND PROCESS EACH ONE

        this.SubDirectories.ForEach(subDirectory => {
            files.AddRange(subDirectory.GetAllFiles());
        });

        return files;

    }

}
