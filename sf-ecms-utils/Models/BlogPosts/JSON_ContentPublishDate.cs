using System.Text.Json.Serialization;

namespace SF_ECMS_Utils.Models;

public class JSON_ContentPublishDate {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    [JsonPropertyName("dateTime")]
    public string DateTime { get; set; }

    [JsonPropertyName("timeZone")]
    public string TimeZone { get; set; }

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public JSON_ContentPublishDate(string DateTime, string TimeZone) {

        this.DateTime = DateTime;
        this.TimeZone = TimeZone;

    }

}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(JSON_ContentPublishDate))]
internal partial class JSONContext_ContentPublishDate : JsonSerializerContext {
}