namespace SF_ECMS_Utils.Models;
public class CMSFile {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public string FileName { get; init; }

    public string FilePath { get; init; }

    public string CMSTitle { get; set; }

    public string CMSMimeType { get; init; }

    public string? CMSContentKey { get; init; }

    public string CMSPath { get; init; }

    public CSV_SummarizedCMSFile SummarizedValues => new() {
        CMSContentKey = this.CMSContentKey ?? "",
        CMSTitle = this.CMSTitle ?? "",
        CMSPath = this.CMSPath ?? "",
        FileName = this.FileName ?? "",
        CMSMimeType = this.CMSMimeType ?? ""
    };

    public JSON_Content CMSContentJSON => new(
        type: "sfdc_cms__document",
        title: this.CMSTitle,
        contentBody: new(
            sfdc_cms_media: new(
                source: new(
                    _type: "file",
                    mimeType: this.CMSMimeType
                )
            )
        )
    );

    public JSON_Meta CMSMetaJSON => new(
        contentKey: this.CMSContentKey,
        path : this.CMSPath
    );

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public CMSFile(string fileName, string cmsTitle, string filePath, string cmsPath, string cmsMimeType, string? CMSContentKey = null) {

        this.FileName = fileName;
        this.FilePath = filePath;
        this.CMSTitle = cmsTitle;
        this.CMSMimeType = cmsMimeType;
        this.CMSContentKey = CMSContentKey;
        this.CMSPath = cmsPath;

    }

    /***********************************************************************************************************/
    /************************************************* METHODS *************************************************/
    /***********************************************************************************************************/

}