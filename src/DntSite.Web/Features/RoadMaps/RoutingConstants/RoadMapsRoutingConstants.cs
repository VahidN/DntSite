namespace DntSite.Web.Features.RoadMaps.RoutingConstants;

public static class RoadMapsRoutingConstants
{
    public const string LearningPaths1 = "/LearningPaths";
    public const string LearningPaths2 = "/learning-paths";

    public const string LearningPathsPageCurrentPage = $"{LearningPaths2}/page/{{CurrentPage:int?}}";

    public const string LearningPathsFilterBase = $"{LearningPaths2}/filter";

    public const string LearningPathsFilterFilterPageCurrentPage =
        $"{LearningPathsFilterBase}/{{Filter}}/page/{{CurrentPage:int?}}";

    public const string LearningPathsDetailsLearningPathId1 = "/LearningPaths/Details/{LearningPathId:int}";

    public const string LearningPathsDetailsBase = $"{LearningPaths2}/details";
    public const string LearningPathsDetailsLearningPathId2 = $"{LearningPathsDetailsBase}/{{LearningPathId:int}}";

    public const string LearningPathsTag = "/learning-paths-tag";
    public const string LearningPathsTagPageCurrentPage = $"{LearningPathsTag}/page/{{CurrentPage:int?}}";
    public const string LearningPathsTagTagName = $"{LearningPathsTag}/{{TagName}}";

    public const string LearningPathsTagTagNamePageCurrentPage =
        $"{LearningPathsTag}/{{TagName}}/page/{{CurrentPage:int?}}";

    public const string LearningPathsWriters = "/learning-paths-writers";
    public const string LearningPathsWritersPageCurrentPage = $"{LearningPathsWriters}/page/{{CurrentPage:int?}}";
    public const string LearningPathsWritersUserFriendlyName = $"{LearningPathsWriters}/{{UserFriendlyName}}";

    public const string LearningPathsWritersUserFriendlyNamePageCurrentPage =
        $"{LearningPathsWriters}/{{UserFriendlyName}}/page/{{CurrentPage:int?}}";

    public const string WriteLearningPath = "/write-learning-path";

    public const string WriteLearningPathEditBase = $"{WriteLearningPath}/edit";

    public const string WriteLearningPathEditEditId =
        $"{WriteLearningPathEditBase}/{{EditId:{EncryptedRouteConstraint.Name}}}";

    public const string WriteLearningPathDeleteBase = $"{WriteLearningPath}/delete";

    public const string WriteLearningPathDeleteDeleteId =
        $"{WriteLearningPathDeleteBase}/{{DeleteId:{EncryptedRouteConstraint.Name}}}";

    public const string CommentsUrlTemplate = $"{LearningPathsDetailsBase}/{{0}}#comments";
    public const string PostUrlTemplate = $"{LearningPathsDetailsBase}/{{0}}";
    public const string PostTagUrlTemplate = $"{LearningPathsTag}/{{0}}";
    public const string EditPostUrlTemplate = $"{WriteLearningPathEditBase}/{{0}}";
    public const string DeletePostUrlTemplate = $"{WriteLearningPathDeleteBase}/{{0}}";
}
