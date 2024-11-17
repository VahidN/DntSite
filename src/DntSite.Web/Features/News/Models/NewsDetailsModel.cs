using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.Models;

public class NewsDetailsModel
{
    public DailyNewsItem? CurrentNews { set; get; }

    public DailyNewsItem? NextNews { set; get; }

    public DailyNewsItem? PreviousNews { set; get; }

    public IList<DailyNewsItemComment> CommentsList { set; get; } = [];
}
