using System.Text.Json.Serialization;

namespace SF_Import_Builder.Models;

public class JSON_Content_ContentBody {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    [JsonPropertyName("sfdc_cms:media")]
    public JSON_Content_SFDCMedia sfdc_cms_media { get; set; }

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public JSON_Content_ContentBody(JSON_Content_SFDCMedia sfdc_cms_media) {

        this.sfdc_cms_media = sfdc_cms_media;

    }

}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(JSON_Content_ContentBody))]
internal partial class JSON_Content_ContentBodyContext : JsonSerializerContext {
}