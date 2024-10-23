using System.Text.Json.Serialization;

namespace SF_ECMS_Utils.Models;

public class JSON_ContentContentBody_BlogPost {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public string AuthorName { get; set; }
    public string Body { get; set; }
    public string Category { get; set; }
    public string Excerpt { get; set; }
    public string Image_URL { get; set; }
    public string Status { get; set; }
    public JSON_ContentPublishDate PublishDate { get; set; }
    public string Title { get; set; }
    public string Banner_Image_URL { get; set; }
    public string Legacy { get; set; }

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public JSON_ContentContentBody_BlogPost(string AuthorName, string Body, string Category, string Excerpt, string Image_URL, string Status, JSON_ContentPublishDate PublishDate, string Title, string Banner_Image_URL, string Legacy) {

        this.AuthorName = AuthorName;
        this.Body = Body;
        this.Category = Category;
        this.Excerpt = Excerpt;
        this.Image_URL = Image_URL;
        this.Status = Status;
        this.PublishDate = PublishDate;
        this.Title = Title;
        this.Banner_Image_URL = Banner_Image_URL;
        this.Legacy = Legacy;

    }

}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(JSON_ContentContentBody_BlogPost))]
internal partial class JSONContext_Content_Blog_Post : JsonSerializerContext {
}