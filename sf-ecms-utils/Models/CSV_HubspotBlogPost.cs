using CsvHelper.Configuration.Attributes;
namespace SF_ECMS_Utils.Models;

public class CSV_HubspotBlogPost {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    [Name("Title")]
    public string Title { get; set; } = "";

    [Name("AuthorName")]
    public string AuthorName { get; set; } = "";

    [Name("Body")]
    public string Body { get; set; } = "";

    [Name("Tags")]
    public string Tags { get; set; } = "";

    [Name("Excerpt")]
    public string Excerpt { get; set; } = "";

    [Name("Image_URL")]
    public string Image_URL { get; set; } = "";

    [Name("PublishDate")]
    public string PublishDate { get; set; } = "";

    [Name("URL")]
    public string URL { get; set; } = "";

    [Name("PostID")]
    public string PostID { get; set; } = "";

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

}