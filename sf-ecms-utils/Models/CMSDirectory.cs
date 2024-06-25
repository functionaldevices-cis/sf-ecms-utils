using System.Collections.Generic;
using System.IO;

namespace SF_ECMS_Utils.Models; 
public class CMSDirectory {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public string DirectoryName { get; init; }
    public string DirectoryPath { get; init; }
    public string CMSPath { get; init; }
    public List<CMSDirectory> SubDirectories { get; init; }
    public List<CMSFile> Files { get; init; }



    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public CMSDirectory(string directoryName, string directoryPath, string cmsPath) {

        this.DirectoryName = directoryName;
        this.DirectoryPath = directoryPath;
        this.CMSPath = cmsPath;
        this.SubDirectories = [];
        this.Files = [];

    }

    /***********************************************************************************************************/
    /************************************************* METHODS *************************************************/
    /***********************************************************************************************************/

    public List<CMSFile> GetAllFiles () {

        // GET THE FILES THAT ARE DIRECTLY IN THIS DIRECTORY

        List<CMSFile> files = [];
        
        files.AddRange(this.Files);

        // LOOP THROUGH THE SUBDIRECTORIES AND GET EACH

        this.SubDirectories.ForEach(subDirectory => {
            files.AddRange(subDirectory.GetAllFiles());
        });

        return files;

    }

}
