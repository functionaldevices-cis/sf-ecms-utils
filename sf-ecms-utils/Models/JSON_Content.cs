using System.Text.Json.Serialization;

namespace SF_ECMS_Utils.Models;

public class JSON_Content {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public string type { get; set; }

    public string title { get; set; }

    public JSON_Content_ContentBody contentBody { get; set; }

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public JSON_Content(string type, string title, JSON_Content_ContentBody contentBody) {

        this.type = "sfdc_cms__document";
        this.title = title;
        this.contentBody = contentBody;

    }

}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(JSON_Content))]
internal partial class JSON_ContentContext : JsonSerializerContext {
}