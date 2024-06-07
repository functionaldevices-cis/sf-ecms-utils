using System.Text.Json.Serialization;

namespace SF_Import_Builder.Models;

public class JSON_Content_Source {

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

    public JSON_Content_Source(string _type, string mimeType, string? _ref = null, int? size = null) {

        this._type = _type;
        this.mimeType = mimeType;
        this._ref = _ref;
        this.size = size;

    }

}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(JSON_Content_Source))]
internal partial class JSON_Content_SourceContext : JsonSerializerContext {
}