namespace SF_Import_Builder.Models;
public class ImporteeFile {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public string File_Name { get; init; }

    public string File_Path { get; init; }

    public string Content_Title { get; init; }

    public string Content_MimeType { get; init; }

    public string? Meta_ContentKey { get; init; }

    public string Meta_Path { get; init; }

    public JSON_Content Content_JSON => new(
        type: "sfdc_cms__document",
        title: this.Content_Title,
        contentBody: new() {
            {
                "sfdc_cms:media", new() {
                    {
                        "source", new() {
                            {
                                "type",
                                "file"
                            },
                            {
                                "mimeType",
                                this.Content_MimeType
                            }
                        }
                    }
                }
            }
        }
    );

    public JSON_Meta Meta_JSON => new(
        contentKey: this.Meta_ContentKey,
        path : this.Meta_Path
    );

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public ImporteeFile(string file_Name, string content_Title, string file_Path, string meta_Path, string content_MimeType, string? meta_ContentKey = null) {

        this.File_Name = file_Name;
        this.File_Path = file_Path;
        this.Content_Title = content_Title;
        this.Content_MimeType = content_MimeType;
        this.Meta_ContentKey = meta_ContentKey;
        this.Meta_Path = meta_Path;

    }

    /***********************************************************************************************************/
    /************************************************* METHODS *************************************************/
    /***********************************************************************************************************/

}