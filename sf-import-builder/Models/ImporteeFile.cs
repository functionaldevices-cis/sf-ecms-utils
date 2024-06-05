namespace SF_Import_Builder.Models;
public class ImporteeFile {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public string Name { get; init; }

    public string NameWithoutExtension { get; init; }
    public string FullPath { get; init; }
    public string PathWithinRoot { get; init; }
    public string Extension { get; init; }

    public string MimeType => this.Extension switch {
        ".aac" => "audio/aac",
        ".abw" => "application/x-abiword",
        ".apng" => "image/apng",
        ".arc" => "application/x-freearc",
        ".avif" => "image/avif",
        ".avi" => "video/x-msvideo",
        ".azw" => "application/vnd.amazon.ebook",
        ".bin" => "application/octet-stream",
        ".bmp" => "image/bmp",
        ".bz" => "application/x-bzip",
        ".bz2" => "application/x-bzip2",
        ".cda" => "application/x-cdf",
        ".csh" => "application/x-csh",
        ".css" => "text/css",
        ".csv" => "text/csv",
        ".doc" => "application/msword",
        ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        ".eot" => "application/vnd.ms-fontobject",
        ".epub" => "application/epub+zip",
        ".gz" => "application/gzip",
        ".gif" => "image/gif",
        ".htm, .html" => "text/html",
        ".ico" => "image/vnd.microsoft.icon",
        ".ics" => "text/calendar",
        ".jar" => "application/java-archive",
        ".jpeg, .jpg" => "image/jpeg",
        ".js" => "text/javascript",
        ".json" => "application/json",
        ".jsonld" => "application/ld+json",
        ".mid, .midi" => "audio/midi, audio/x-midi",
        ".mjs" => "text/javascript",
        ".mp3" => "audio/mpeg",
        ".mp4" => "video/mp4",
        ".mpeg" => "video/mpeg",
        ".mpkg" => "application/vnd.apple.installer+xml",
        ".odp" => "application/vnd.oasis.opendocument.presentation",
        ".ods" => "application/vnd.oasis.opendocument.spreadsheet",
        ".odt" => "application/vnd.oasis.opendocument.text",
        ".oga" => "audio/ogg",
        ".ogv" => "video/ogg",
        ".ogx" => "application/ogg",
        ".opus" => "audio/opus",
        ".otf" => "font/otf",
        ".png" => "image/png",
        ".pdf" => "application/pdf",
        ".php" => "application/x-httpd-php",
        ".ppt" => "application/vnd.ms-powerpoint",
        ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
        ".rar" => "application/vnd.rar",
        ".rtf" => "application/rtf",
        ".sh" => "application/x-sh",
        ".svg" => "image/svg+xml",
        ".tar" => "application/x-tar",
        ".tif, .tiff" => "image/tiff",
        ".ts" => "video/mp2t",
        ".ttf" => "font/ttf",
        ".txt" => "text/plain",
        ".vsd" => "application/vnd.visio",
        ".wav" => "audio/wav",
        ".weba" => "audio/webm",
        ".webm" => "video/webm",
        ".webp" => "image/webp",
        ".woff" => "font/woff",
        ".woff2" => "font/woff2",
        ".xhtml" => "application/xhtml+xml",
        ".xls" => "application/vnd.ms-excel",
        ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        ".xml" => "application/xml",
        ".xul" => "application/vnd.mozilla.xul+xml",
        ".zip" => "application/zip",
        ".3gp" => "audio/3gpp",
        ".3g2" => "audio/3gpp2",
        ".7z" => "application/x-7z-compressed",
        _ => "application/octet-stream"
    };

    public JSON_Content ContentJSON => new(
        title: this.NameWithoutExtension,
        mimeType: this.MimeType
    );

    public JSON_Meta MetaJSON => new(
        path : this.PathWithinRoot
    );

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public ImporteeFile(string name, string nameWithoutExtension, string fullPath, string pathWithinRoot, string extension) {

        this.Name = name;
        this.NameWithoutExtension = nameWithoutExtension;
        this.FullPath = fullPath;
        this.PathWithinRoot = pathWithinRoot;
        this.Extension = extension;

    }

    /***********************************************************************************************************/
    /************************************************* METHODS *************************************************/
    /***********************************************************************************************************/

}