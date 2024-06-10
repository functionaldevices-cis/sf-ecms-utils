# Salesforce "Enhanced CMS" Utilities (SF ECMS Utils)
 
This is a .NET 8 program written to help manage SF ECMS instances that contain large numbers of files. It can:

1. Scan, format, and create zipped import packages (containing all required files such as `content.json` and `_meta.json` files).
   * Unlimited nested subfolders are supported. Your ECMS will create any needed folders based on information in the import ZIP file.
3. Scan and analyze formatted folders, such as unzipped exports from an ECMS.

The JSON files are created based on the specification provided by Salesforce here: https://help.salesforce.com/s/articleView?id=sf.cms_import_content_json_enhanced.htm

# Configuration

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
* `CreateZipFile`
  * **Required**: `False`
  * **Default**: `False`
  * **Type**: Boolean
  * **Description**: Flag to control whether or not the program creates a zip file after it finishes creating a packaged folder.

# Examples

### Example 1
[Unzip and Analyze an Exported Zip File](https://github.com/functionaldevices-cis/sf-import-builder/tree/main/Examples/Example%201)

### Example 2
[Analyze an Already Unzipped Export](https://github.com/functionaldevices-cis/sf-import-builder/tree/main/Examples/Example%202)

### Example 3
[Package a set of Files for Import](https://github.com/functionaldevices-cis/sf-import-builder/tree/main/Examples/Example%203)
