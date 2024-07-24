namespace DntSite.Web.Features.Stats.Models;

public class PostsArchiveFavUserRatings
{
    public IList<SectionFavUserRating>? TodayFavUserRatings { set; get; }

    public IList<SectionFavUserRating>? YesterdayFavUserRatings { set; get; }

    public IList<SectionFavUserRating>? AllFavUserRatings { set; get; }

    public IList<SectionFavUserRating>? LastMonthFavUserRatings { set; get; }
}
