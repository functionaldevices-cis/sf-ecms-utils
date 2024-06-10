# Salesforce Enhanced CMS Import ZIP Builder
 
This is a .NET 8 program which will automatically create an import ZIP package for Salesforce Enhanced CMSs. The ZIP file will contain individual folders for every importee file, containing the file, the `content.json`, and the `_meta.json`. Please note, any subfolders within the specified source folder will be automatically created in your Enhanced CMS space, and the folders will be uploaded into the matching folder structure.

The JSON files are created based on the specification provided by Salesforce here: https://help.salesforce.com/s/articleView?id=sf.cms_import_content_json_enhanced.htm

# Configuration

You configure the program by filling in the `sf-import-builder-config.json` file that comes with the program.

* `SourceFilterPath`
  * **Required**: `True`
  * **Type**: `String`
  * **Description**: The full windows path of the source folder.
* `OutputFolderPath`
  * **Required**: `True`
  * **Type**: `String`
  * **Description**: The full windows path of the output folder.
* `CreateZipPackage`
  * **Required**: `False`
  * **Default**: `True`
  * **Type**: Boolean
  * **Description**: Flag to control whether or not the program makes a ZIP package after it creates the new folder structure and files in the output folder.
* `DeleteOutputFolders`
  * **Required**: `False`
  * **Default**: `False`
  * **Type**: Boolean
  * **Description**: Flag to control whether or not the program deletes the output folder after it finishes running. Useful if you only care about the final ZIP package.

# Examples

See here for example: https://github.com/functionaldevices-cis/sf-import-builder/tree/main/Examples
