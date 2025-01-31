using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Common.EmailLayouts;

public partial class EmailsLayoutSignature
{
    private BaseEmailModel? _model;

    [Inject] internal ICachedAppSettingsProvider CachedAppSettingsProvider { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var settings = await CachedAppSettingsProvider.GetAppSettingsAsync();

        _model = new BaseEmailModel
        {
            EmailSig = settings.SiteEmailsSig ?? "",
            SiteRootUri = settings.SiteRootUri,
            SiteTitle = settings.BlogName,
            MsgDateTime = DateTime.UtcNow.ToLongPersianDateTimeString().ToPersianNumbers()
        };
    }
}
