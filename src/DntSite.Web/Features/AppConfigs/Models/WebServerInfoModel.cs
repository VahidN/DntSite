using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace DntSite.Web.Features.AppConfigs.Models;

public class WebServerInfoModel
{
    public required WebServerInfo ServerInfo { set; get; }

    public required string VersionInfo { set; get; }

    public required List<IKey> KeysList { set; get; }

    public required IWebHostEnvironment WebHostEnvironment { set; get; }
}
