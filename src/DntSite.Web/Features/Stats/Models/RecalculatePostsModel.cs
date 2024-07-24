namespace DntSite.Web.Features.Stats.Models;

public class RecalculatePostsModel
{
    public int NumberOfQuestions { set; get; }

    public int NumberOfBacklogs { set; get; }

    public int NumberOfPosts { set; get; }

    public int NumberOfTags { set; get; }

    public int NumberOfAdmins { set; get; }

    public int NumberOfUsers { set; get; }

    public int NumberOfWriters { set; get; }

    public int NumberOfLinks { set; get; }

    public IList<Browser>? BrowsersList { set; get; }

    public int OnlineVisitors { set; get; }

    public int NumberOfTodayBirthdays { set; get; }

    public int NumberOfProjects { set; get; }

    public int NumberOfCourses { set; get; }

    public int NumberOfVotes { set; get; }

    public int NumberOfComingSoon { set; get; }

    public int NumberOfAds { set; get; }

    public int NumberOfJobsSeekers { set; get; }

    public int NumberOfLearningPaths { set; get; }

    public SiteStatsModel? SiteStats { set; get; }
}
