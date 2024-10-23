using System.Text.Json.Serialization;

namespace SF_ECMS_Utils.Models;

public class JSON_ContentSource {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    [JsonPropertyName("type")]
    public string _type { get; set; }

    public string mimeType { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ref")]
    public string? _ref { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? size { get; set; }

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public JSON_ContentSource(string _type, string mimeType, string? _ref = null, int? size = null) {

        this._type = _type;
        this.mimeType = mimeType;
        this._ref = _ref;
        this.size = size;

    }

}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(JSON_ContentSource))]
internal partial class JSONContext_ContentSource : JsonSerializerContext {
}