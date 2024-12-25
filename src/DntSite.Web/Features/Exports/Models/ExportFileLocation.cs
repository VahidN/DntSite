namespace DntSite.Web.Features.Exports.Models;

public class ExportFileLocation
{
    public required string OutputPdfFileName { set; get; }

    public required string OutputFolder { set; get; }

    public required string OutputPdfFilePath { set; get; }

    public required string OutputPdfFileSize { set; get; }

    public required string OutputPdfFileUrl { set; get; }

    public required bool IsReady { set; get; }
}
