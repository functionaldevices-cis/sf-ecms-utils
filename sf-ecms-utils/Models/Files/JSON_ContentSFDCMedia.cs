using System.Text.Json.Serialization;

namespace SF_ECMS_Utils.Models;

public class JSON_ContentSFDCMedia {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public JSON_ContentSource source { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? url { get; set; }

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public JSON_ContentSFDCMedia(JSON_ContentSource source, string? url = null) {

        this.source = source;
        this.url = url;

    }

}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(JSON_ContentSFDCMedia))]
internal partial class JSONContext_ContentSFDCMedia : JsonSerializerContext {
}