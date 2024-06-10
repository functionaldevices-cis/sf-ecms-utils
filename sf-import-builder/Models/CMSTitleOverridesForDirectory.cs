namespace SF_Import_Builder.Models;

public class CMSTitleOverridesForDirectory {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public CMSTitleOverride? DefaultOverride { get; set; }

    public List<CMSTitleOverride> PerFileOverrides { get; set; }

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public CMSTitleOverridesForDirectory(CMSTitleOverride? defaultOverride = null, List<CMSTitleOverride>? perFileOverrides = null) {

        this.DefaultOverride = defaultOverride;
        this.PerFileOverrides = perFileOverrides ?? [];

    }

    /***********************************************************************************************************/
    /************************************************* METHODS *************************************************/
    /***********************************************************************************************************/

    public string? GetOverrideForFile(string fileName) {

        List<CMSTitleOverride> matchingCMSTitles = this.PerFileOverrides.Where(overRide => overRide.FileName == fileName).ToList();

        if (matchingCMSTitles.Count > 0) {
            return matchingCMSTitles[0].CMSTitle;
        } else if (this.DefaultOverride != null) {
            return this.DefaultOverride.CMSTitle;
        }

        return null;

    }

}