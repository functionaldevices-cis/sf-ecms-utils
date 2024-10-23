using CsvHelper.Configuration.Attributes;

namespace SF_ECMS_Utils.Models;
public class CMSBlogPost {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public string ID { get; set; } = "";

    public string Type { get; set; } = "";

    public string Title { get; set; } = "";

    public string AuthorName { get; set; } = "";

    public string Body { get; set; } = "";

    public string Category { get; set; } = "";

    public string Excerpt { get; set; } = "";

    public string Image_URL { get; set; } = "";

    public string Status { get; set; } = "";

    public string DateTime { get; set; } = "";

    public string TimeZone { get; set; } = "";

    public string OldURL { get; set; } = "";

    public string? CMSContentKey { get; init; }

    public string CMSPath { get; init; }

    public CSV_BlogPost SummarizedValues => new() {
        ID = this.ID,
        CMSContentKey = this.CMSContentKey ?? "",
        CMSPath = this.CMSPath,
        Type = this.Type,
        Title = this.Title,
        AuthorName = this.AuthorName,
        Body = this.Body,
        Category = this.Category,
        Excerpt = this.Excerpt,
        Image_URL = this.Image_URL,
        Status = this.Excerpt,
        DateTime = this.DateTime,
        TimeZone = this.TimeZone,
        OldURL = this.OldURL
    };

    public JSON_Content_BlogPost CMSContentJSON => new(
        type: this.Type,
        title: this.Title,
        contentBody: new(
            AuthorName: this.AuthorName,
            Body: this.Body,
            Category: this.Category,
            Excerpt: this.Excerpt,
            Image_URL: this.Image_URL,
            Status: this.Status,
            PublishDate: new(
                DateTime: this.DateTime,
                TimeZone: this.TimeZone
            ),
            Title: this.Title,
            Banner_Image_URL: "",
            Legacy: "TRUE"
        )
    );

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public CMSBlogPost(string ID, string? ContentKey, string Type, string Title, string AuthorName, string Body, string Category, string Excerpt, string Image_URL, string Status, string DateTime, string TimeZone, string OldURL) {

        this.ID = ID;
        this.CMSContentKey = ContentKey;
        this.Type = Type;
        this.Title = Title;
        this.AuthorName = AuthorName;
        this.Body = Body;
        this.Category = Category;
        this.Excerpt = Excerpt;
        this.Image_URL = Image_URL;
        this.Status = Status;
        this.DateTime = DateTime;
        this.TimeZone = TimeZone;
        this.OldURL = OldURL;
        this.CMSPath = "";

    }

    /***********************************************************************************************************/
    /************************************************* METHODS *************************************************/
    /***********************************************************************************************************/

}