using System.Text.Json.Serialization;

namespace SF_Import_Builder.Models;

public class JSON_Content {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public string type { get; set; }

    public string title { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public string mimeType { get; set; }

    public Dictionary<string, Dictionary<string, Dictionary<string, string>>> contentBody => new() {
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
                            this.mimeType
                        }
                    }
                }
            }
        }
    };

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public JSON_Content(string title, string mimeType) {

        this.type = "sfdc_cms__document";
        this.title = title;
        this.mimeType = mimeType;

    }

}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(JSON_Content))]
internal partial class JSON_ContentContext : JsonSerializerContext {
}
