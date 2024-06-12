using SF_ECMS_Utils.Models;
using System.IO;

namespace SF_ECMS_Utils.Helpers;

public class CMSTitleBuilder
{

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public CSV_CMSTitleOverride? DefaultTitleForDirectory {
        get {
            List<CSV_CMSTitleOverride> matchingCMSTitles = this.TitleOverridesForDirectory.Where(overRide => overRide.FileName == "*").ToList();
            if (matchingCMSTitles.Count > 0) {
                return matchingCMSTitles[0];
            }
            return null;
        }
    }

    public List<CSV_CMSTitleOverride> TitleOverrides { get; set; }

    public string CMSPath { get; set; }

    public List<CSV_CMSTitleOverride> TitleOverridesForDirectory => this.TitleOverrides.Where(overRide => overRide.CMSPath == this.CMSPath).ToList();

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public CMSTitleBuilder(List<CSV_CMSTitleOverride> titleOverrides) {
        
        this.TitleOverrides = titleOverrides;
        this.CMSPath = "";
    
    }

    /***********************************************************************************************************/
    /************************************************* METHODS *************************************************/
    /***********************************************************************************************************/

    public void SetCMSPath(string path) {

        this.CMSPath = path;

    }

    public string GetTitle(string defaultTitle, string fileName) {

        string title = defaultTitle;

        List<CSV_CMSTitleOverride> matchingCMSTitles = this.TitleOverridesForDirectory.Where(overRide => overRide.FileName == fileName).ToList();

        if (this.CMSPath=="Flyers, Handouts, Etc") {

        }

        if (matchingCMSTitles.Count > 0)
        {
            title = matchingCMSTitles[0].CMSTitle;
        }
        else if (this.DefaultTitleForDirectory != null)
        {
            title = this.DefaultTitleForDirectory.CMSTitle;
            if (this.CMSPath.StartsWith("Diagrams")) {

            }
        }

        return title.Replace("[FILENAME]", defaultTitle);

    }

}