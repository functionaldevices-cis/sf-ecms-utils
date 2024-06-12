using CsvHelper.Configuration.Attributes;

namespace SF_ECMS_Utils.Models;

public class CSV_SummarizedCMSFile {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    [Name("CMS Content Key")]
    public string CMSContentKey { get; set; } = "";

    [Name("CMS Title")]
    public string CMSTitle { get; set; } = "";

    [Name("CMS Folder Path")]
    public string CMSPath { get; set; } = "";

    [Name("File Name")]
    public string FileName { get; set; } = "";

    [Name("MimeType")]
    public string CMSMimeType { get; set; } = "";

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

}