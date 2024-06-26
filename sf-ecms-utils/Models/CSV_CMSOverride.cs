﻿using CsvHelper.Configuration.Attributes;

namespace SF_ECMS_Utils.Models;

public class CSV_CMSOverride {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    [Name("CMS Path")]
    public string CMSPath { get; set; } = "";

    [Name("CMS Title")]
    public string CMSTitle { get; set; } = "";

    [Name("CMS Type")]
    public string CMSType { get; set; } = "";

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

}