using System.Text;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Services;

public class HumansTxtFileService(IUsersInfoService usersService, IAppFoldersService appFoldersService)
    : IHumansTxtFileService
{
    /// <summary>
    ///     More info: http://humanstxt.org/Standard.html
    /// </summary>
    public async Task CreateHumansTxtFileAsync()
    {
        var content = await CreateHumansListAsync();
        var path = Path.Combine(appFoldersService.WwwRootPath, path2: "humans.txt");
        await File.WriteAllTextAsync(path, content, Encoding.UTF8);
    }

    private async Task<string> CreateHumansListAsync()
    {
        var list = await usersService.GetActiveArticleWritersListAsync(count: 1000);
        var sb = new StringBuilder();
        sb.AppendLine(value: "/* TEAM */");

        foreach (var item in list)
        {
            if (item is null)
            {
                continue;
            }

            sb.AppendLine("   Writer: " + item.FriendlyName);
            sb.AppendLine(value: "   From: Iran\n");
        }

        sb.AppendLine(value: "\n\n/* SITE */");
        sb.AppendLine("   Last update: " + DateTime.UtcNow.ToShortDateString());
        sb.AppendLine(value: "   Language: Persian");
        sb.AppendLine(value: "   Doctype: HTML5");
        sb.AppendLine(value: "   IDE: Rider");

        sb.AppendLine(value: "   Components: Entity Framework, Html Agility Pack, ITextSharp, ASP.NET, Blazor");

        var content = sb.ToString();

        return content;
    }
}
