using System.Text.Json.Serialization;

namespace SF_ECMS_Utils.Models;

public class JSON_Meta {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? contentKey { get; set; }
    public string path { get; set; }

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public JSON_Meta(string? contentKey = null, string? path = null) {

        this.contentKey = contentKey;
        this.path = path ?? "";

    }


}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(JSON_Meta))]
internal partial class JSON_MetaContext : JsonSerializerContext {
}
