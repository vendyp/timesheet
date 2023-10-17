namespace TimesheetService.Domain.Extensions;

public static class FileRepositoryExtensions
{
    public static List<string> ListOfFileTypeImages = new()
    {
        ".PNG",
        ".JPEG",
        ".JPG",
        ".JFIF",
        ".PJPEG",
        ".PJP",
        ".GIF",
        ".SVG",
        ".WEBP"
    };

    public static List<string> ListOfFileTypeDocuments = new()
    {
        ".TXT",
        ".DOCX",
        ".DOC",
        ".DOM",
        ".DOT",
        ".JSON",
        ".PPTX",
        ".PPT",
        ".CSV",
        ".XLS",
        ".XLSX"
    };
}