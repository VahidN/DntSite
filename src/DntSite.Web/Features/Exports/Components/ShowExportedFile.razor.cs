using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Exports.Components;

public partial class ShowExportedFile
{
    private ExportFileLocation? _exportFileLocation;

    [InjectComponentScoped] internal IPdfExportService PdfExportService { set; get; } = null!;

    [EditorRequired] [Parameter] public required int Id { set; get; }

    [EditorRequired] [Parameter] public required WhatsNewItemType ItemType { set; get; }

    private string Title => string.Create(CultureInfo.InvariantCulture,
        $"دریافت نگارش PDF مطلب با حجم {_exportFileLocation?.OutputPdfFileSize}");

    protected override async Task OnInitializedAsync()
        => _exportFileLocation = await PdfExportService.GetExportFileLocationAsync(ItemType, Id);
}
