namespace DntSite.Web.Features.RoadMaps.Models;

public class LearningPathLinksModel
{
    public int Id { set; get; }

    public required string Title { set; get; }

    public required IList<string> Tags { set; get; } = [];

    public IList<int> PostIds { set; get; } = [];

    public IList<Guid> CourseTopicIds { set; get; } = [];

    public IList<int> QuestionIds { set; get; } = [];
}
