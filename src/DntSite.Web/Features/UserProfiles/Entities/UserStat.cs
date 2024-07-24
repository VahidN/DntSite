namespace DntSite.Web.Features.UserProfiles.Entities;

[ComplexType]
public class UserStat
{
    public int NumberOfPosts { set; get; }

    public int NumberOfComments { set; get; }

    public int NumberOfLinks { set; get; }

    public int NumberOfProjects { set; get; }

    public int NumberOfDrafts { set; get; }

    public int NumberOfProjectsFeedbacks { set; get; }

    public int NumberOfProjectsComments { set; get; }

    public int NumberOfLinksComments { set; get; }

    public int NumberOfSurveys { set; get; }

    public int NumberOfSurveyComments { set; get; }

    public int NumberOfAdvertisements { set; get; }

    public int NumberOfAdvertisementComments { set; get; }

    public int NumberOfCourses { set; get; }

    public int NumberOfLearningPaths { set; get; }

    public int NumberOfStackExchangeQuestions { set; get; }

    public int NumberOfStackExchangeQuestionsComments { set; get; }

    public int NumberOfBacklogs { set; get; }
}
