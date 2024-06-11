using SF_ECMS_Utils.Models;
using System.IO;

namespace SF_ECMS_Utils.Helpers;

public class CMSTitleBuilder
{

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public CMSTitleOverride? DefaultTitleForDirectory {
        get {
            List<CMSTitleOverride> matchingCMSTitles = this.TitleOverridesForDirectory.Where(overRide => overRide.FileName == "*").ToList();
            if (matchingCMSTitles.Count > 0) {
                return matchingCMSTitles[0];
            }
            return null;
        }
    }

    public List<CMSTitleOverride> TitleOverrides { get; set; }

    public string CMSPath { get; set; }

    public List<CMSTitleOverride> TitleOverridesForDirectory => this.TitleOverrides.Where(overRide => overRide.CMSPath == this.CMSPath).ToList();

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public CMSTitleBuilder(List<CMSTitleOverride> titleOverrides) {
        
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

        List<CMSTitleOverride> matchingCMSTitles = TitleOverrides.Where(overRide => overRide.FileName == fileName).ToList();

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

        return title;

    }

}