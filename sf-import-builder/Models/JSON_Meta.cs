using System.Text.Json.Serialization;

namespace SF_Import_Builder.Models;

public class JSON_Meta {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public string path { get; set; }

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public JSON_Meta(string path) {

        this.path = path.Replace('\\', '/');

    }

}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(JSON_Meta))]
internal partial class JSON_MetaContext : JsonSerializerContext {
}
