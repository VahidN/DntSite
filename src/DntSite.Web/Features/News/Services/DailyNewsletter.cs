using System.Text;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.RssFeeds.Services.Contracts;

namespace DntSite.Web.Features.News.Services;

public class DailyNewsletter(IFeedsService feedsService) : IDailyNewsletter
{
    private const string Hr = "<hr style='border:none;border-bottom:solid #EEEEFF 1.0pt;padding:0'>";

    private const string GroupStyle =
        "style='background: lightslategray;color: white;border-radius: 4px;padding: 2px;margin-left: 5px;'";

    // It runs in an http context less environment.
    public async Task<string> GetEmailContentAsync(DateTime fromDateTime, bool showBriefDescription)
    {
        var posts = (await feedsService.GetLatestChangesAsync(showBriefDescription)).RssItems?.Where(x
            => x.PublishDate >= new DateTimeOffset(fromDateTime));

        if (posts is null)
        {
            return string.Empty;
        }

        var data = new StringBuilder();

        foreach (var post in posts.OrderBy(x => x.ItemType.Value).ThenBy(x => x.PublishDate))
        {
            var group = post.ItemType.Value;

            if (!string.IsNullOrWhiteSpace(group))
            {
                data.AppendFormat(CultureInfo.InvariantCulture, format: "<b {1}>{0}</b>", group, GroupStyle);
            }

            data.AppendFormat(CultureInfo.InvariantCulture, format: "<a dir='{2}' href='{0}'><b>{1}</b></a><br/>",
                post.Url, post.Title.SanitizeXmlString(), post.Title.GetDir());

            data.AppendFormat(CultureInfo.InvariantCulture, format: "<b>{0}</b>{1}<br/>", post.User!.FriendlyName, Hr);

            var contentDir = post.Content.GetDir();

            var contentAlign = string.Equals(contentDir, b: "ltr", StringComparison.OrdinalIgnoreCase)
                ? "align='left'"
                : "align='right'";

            data.AppendFormat(CultureInfo.InvariantCulture, format: "<div {2} dir='{0}'>{1}</div>", contentDir,
                post.Content, contentAlign);

            data.AppendFormat(CultureInfo.InvariantCulture, format: "<br>{0}<br><br>", Hr);
        }

        return data.ToString();
    }
}
