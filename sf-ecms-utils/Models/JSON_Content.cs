using System.Text.Json.Serialization;

namespace SF_ECMS_Utils.Models;

public class JSON_Content {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public string type { get; set; }

    public string title { get; set; }

    public JSON_ContentContentBody contentBody { get; set; }

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public JSON_Content(string type, string title, JSON_ContentContentBody contentBody) {

        this.type = type;
        this.title = title;
        this.contentBody = contentBody;

    }

}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(JSON_Content))]
internal partial class JSON_ContentContext : JsonSerializerContext {
}