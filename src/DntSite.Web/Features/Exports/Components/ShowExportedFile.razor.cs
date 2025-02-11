using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Exports.Components;

public partial class ShowExportedFile
{
    private ExportFileLocation? _exportFileLocation;

    [InjectComponentScoped] internal IPdfExportService PdfExportService { set; get; } = null!;

    [EditorRequired] [Parameter] public required int Id { set; get; }

    [Parameter] public string? Caption { set; get; }

    [Parameter] public string ButtonClass { get; set; } = "btn-outline-secondary";

    [EditorRequired] [Parameter] public WhatsNewItemType? ItemType { set; get; }

    private string Title => string.Create(CultureInfo.InvariantCulture,
        $"دریافت نگارش PDF با حجم {_exportFileLocation?.OutputPdfFileSize}");

    protected override async Task OnParametersSetAsync()
        => _exportFileLocation = await PdfExportService.GetExportFileLocationAsync(ItemType, Id);
}
