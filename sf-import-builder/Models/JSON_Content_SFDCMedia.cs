using System.Text.Json.Serialization;

namespace SF_Import_Builder.Models;

public class JSON_Content_SFDCMedia {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public JSON_Content_Source source { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? url { get; set; }

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public JSON_Content_SFDCMedia(JSON_Content_Source source, string? url = null) {

        this.source = source;
        this.url = url;

    }

}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(JSON_Content_SFDCMedia))]
internal partial class JSON_Content_SFDCMediaContext : JsonSerializerContext {
}