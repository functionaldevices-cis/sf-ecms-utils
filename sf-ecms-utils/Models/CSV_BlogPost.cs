using CsvHelper.Configuration.Attributes;

namespace SF_ECMS_Utils.Models;

public class CSV_BlogPost {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    [Name("ID")]
    public string ID { get; set; } = "";

    [Name("CMS Content Key")]
    public string CMSContentKey { get; set; } = "";

    [Name("CMS Path")]
    public string CMSPath { get; set; } = "";

    [Name("Type")]
    public string Type { get; set; } = "";

    [Name("Title")]
    public string Title { get; set; } = "";

    [Name("AuthorName")]
    public string AuthorName { get; set; } = "";

    [Name("Body")]
    public string Body { get; set; } = "";

    [Name("Category")]
    public string Category { get; set; } = "";

    [Name("Excerpt")]
    public string Excerpt { get; set; } = "";

    [Name("Image_URL")]
    public string Image_URL { get; set; } = "";

    [Name("Status")]
    public string Status { get; set; } = "";

    [Name("DateTime")]
    public string DateTime { get; set; } = "";

    [Name("TimeZone")]
    public string TimeZone { get; set; } = "";

    [Name("OLD URL")]
    public string OldURL { get; set; } = "";

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

}