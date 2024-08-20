using System.Text;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.RssFeeds.Services.Contracts;

namespace DntSite.Web.Features.News.Services;

public class DailyNewsletter(IFeedsService feedsService) : IDailyNewsletter
{
    private const string Hr = "<hr style='border:none;border-bottom:solid #EEEEFF 1.0pt;padding:0'>";

    private const string GroupStyle =
        "style='background: grey;color: white;border-radius: 4px;padding: 2px;margin-left: 5px;'";

    // It runs in a http context less environment.
    public async Task<string> GetEmailContentAsync(string url, DateTime yesterday)
    {
        var posts = (await feedsService.GetLatestChangesAsync()).RssItems?.Where(x
            => x.PublishDate.Year == yesterday.Year && x.PublishDate.Month == yesterday.Month &&
               x.PublishDate.Day == yesterday.Day);

        if (posts is null)
        {
            return string.Empty;
        }

        var data = new StringBuilder();

        foreach (var post in posts)
        {
            var group = post.Categories.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(group))
            {
                data.AppendFormat(CultureInfo.InvariantCulture, format: "<b {1}>{0}</b>", group, GroupStyle);
            }

            data.AppendFormat(CultureInfo.InvariantCulture, format: "<a dir='{2}' href='{0}'><b>{1}</b></a><br/>",
                post.Url, post.Title, post.Title.GetDir());

            data.AppendFormat(CultureInfo.InvariantCulture, format: "<b>{0}</b>{1}<br/>", post.User!.FriendlyName, Hr);

            data.AppendFormat(CultureInfo.InvariantCulture, format: "<div dir='{0}'>{1}</div>", post.Content.GetDir(),
                post.Content);

            data.AppendFormat(CultureInfo.InvariantCulture, format: "<br>{0}<br><br>", Hr);
        }

        return data.ToString();
    }
}
