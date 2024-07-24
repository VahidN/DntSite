using DntSite.Web.Features.AppConfigs.Models;
using Microsoft.Extensions.Options;

namespace DntSite.Web.Features;

public partial class App
{
    [Inject] private IOptionsSnapshot<StartupSettingsModel> SiteSettings { set; get; } = null!;
}
