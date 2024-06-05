using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Import_Builder.Models;
public class Config {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public string SourceFolderPath { get; set; }
    public string OutputFolderPath { get; set; }
    public bool CreateImportZip { get; set; }
    public bool DeleteImportFolder { get; set; }

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public Config(string SourceFolderPath, string? OutputFolderPath = null, bool CreateImportZip = false, bool DeleteImportFolder = false) {
        
        this.SourceFolderPath = SourceFolderPath;
        this.OutputFolderPath = OutputFolderPath ?? SourceFolderPath;
        this.CreateImportZip = CreateImportZip;
        this.DeleteImportFolder = DeleteImportFolder;

    }

    /***********************************************************************************************************/
    /************************************************* METHODS *************************************************/
    /***********************************************************************************************************/

}
