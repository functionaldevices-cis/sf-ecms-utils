# Salesforce "Enhanced CMS" Utilities (SF ECMS Utils)
 
This is a .NET 8 program written to help manage SF ECMS instances that contain large numbers of files. It can:

1. Scan, format, and create zipped import packages (containing all required files such as `content.json` and `_meta.json` files).
   * Unlimited nested subfolders are supported. Your ECMS will create any needed folders based on information in the import ZIP file.
   * CMS Titles will default to the file name without extension, and CMS Type will default to either `sfdc_cms__document` or `sfdc_cms__image` based on the file type, buth both can be overridden per-file, or per-folder, by adding a file called `sfcu_overrides.csv` in the source root folder. See example 3 for details.
3. Scan and analyze formatted folders, such as unzipped exports from an ECMS.

The JSON files are created based on the specification provided by Salesforce here: https://help.salesforce.com/s/articleView?id=sf.cms_import_content_json_enhanced.htm

# Configuration

### Required Configuration File In Folder with Executable

You configure the program by filling in the `sf-ecms-utils-config.json` file that comes with the program.

* `Action`
  * **Required**: `True`
  * **Type**: `String`
  * **Description**: The action you are taking. It can either be "AnalyzeFiles", or "PackageFiles".
* `SourceFiles_FolderPath`
  * **Required**: `True|False` (When `Action` = "AnalyzeFiles", this is `False`. When `Action` = "PackageFiles", this is `True`)
  * **Type**: `String`
  * **Description**: The full windows path of the source folder.
* `PackagedFiles_ZipFilePath`
  * **Required**: `True|False` (When `Action` = "AnalyzeFiles", then either `PackagedFiles_ZipFilePath` or `PackagedFiles_FolderPath` must be set. When `Action` = "PackageFiles", then this is `False`)
  * **Type**: `String`
  * **Description**: The full windows path of the output folder.
* `PackagedFiles_FolderPath`
  * **Required**: `True|False` (When `Action` = "AnalyzeFiles", then either `PackagedFiles_ZipFilePath` or `PackagedFiles_FolderPath` must be set. When `Action` = "PackageFiles", then this is `True`)
  * **Default**: `True`
  * **Type**: Boolean
  * **Description**: Flag to control whether or not the program makes a ZIP package after it creates the new folder structure and files in the output folder.
* `CreateZipFiles`
  * **Required**: `False`
  * **Default**: `False`
  * **Type**: Boolean
  * **Description**: Flag to control whether or not the program creates a zip file after it finishes creating a packaged folder.
* `ZipFileSplitLevel`
  * **Required**: `False`
  * **Default**: 0
  * **Type**: Integer
  * **Description**: Determines which level of nested folders will be zipped. By default, `0` will cause the entire "Packaged Files" folder to be zipped into a single "Packaged Files.zip" file. `1` will cause all folders directly within "Packaged Files" to be zipped. This is useful if a single zip file would be too large for Salesforce to import, or if different subfolders contain files with the same filename, which can cause import errors.
 
### Overridding Default Titles and CMS Types

You can override the default CMS Title and CMS Type for single files or entire sub-folders by creating a file named `sfcu_overrides.csv` in the root of the source folder. The structure of the CSV file is as follows:

| CMS Path | CMS Title | CMS Type |
| -- | -- | -- |
| Product Photos | [FILENAME] Product Photo |
| Datasheets | [FILENAME] Datasheet |
| Diagrams | [FILENAME] Wiring Diagram | sfdc_cms__document |
| Bulletins | Bulletin |
| Bulletins/B1622_393193.pdf | Installation Guide |

# Examples

### Example 1
[Unzip and Analyze an Exported Zip File](https://github.com/functionaldevices-cis/sf-import-builder/tree/main/Examples/Example%201)

### Example 2
[Analyze an Already Unzipped Export](https://github.com/functionaldevices-cis/sf-import-builder/tree/main/Examples/Example%202)

### Example 3
[Package a set of Files for Import](https://github.com/functionaldevices-cis/sf-import-builder/tree/main/Examples/Example%203)
