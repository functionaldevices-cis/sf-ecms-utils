using System.Text.Json.Serialization;

namespace SF_ECMS_Utils.Models;

public class JSON_ContentContentBody {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    [JsonPropertyName("sfdc_cms:media")]
    public JSON_ContentSFDCMedia sfdc_cms_media { get; set; }

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public JSON_ContentContentBody(JSON_ContentSFDCMedia sfdc_cms_media) {

        this.sfdc_cms_media = sfdc_cms_media;

    }

}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(JSON_ContentContentBody))]
internal partial class JSONContext_ContentContentBody : JsonSerializerContext {
}