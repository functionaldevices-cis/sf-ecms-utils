using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace SF_ECMS_Utils.Models;

public class JSON_Content_BlogPost {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public string type { get; set; }

    public string title { get; set; }

    public JSON_ContentContentBody_BlogPost contentBody { get; set; }

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public JSON_Content_BlogPost(string type, string title, JSON_ContentContentBody_BlogPost contentBody) {

        this.type = type;
        this.title = title;
        this.contentBody = contentBody;

    }

}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(JSON_Content_BlogPost))]
internal partial class JSON_Content_BlogPostContext : JsonSerializerContext {
}