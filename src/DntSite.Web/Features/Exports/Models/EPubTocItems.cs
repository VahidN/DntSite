namespace DntSite.Web.Features.Exports.Models;

public record EPubTocItems(
    int ArticlesCount,
    int AuthorsCount,
    int ArticleGroupsCount,
    int LearningPathsCount,
    int CoursesCount,
    int NewsCount);
