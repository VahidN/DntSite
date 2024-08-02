namespace DntSite.Web.Features.Courses.RoutingConstants;

public static class CoursesRoutingConstants
{
    public const string Courses = "/courses";
    public const string CoursesPageCurrentPage = "/courses/page/{CurrentPage:int}";
    public const string CoursesFilterBase = "/courses/filter";
    public const string CoursesFilterFilterPageCurrentPage = $"{CoursesFilterBase}/{{Filter}}/page/{{CurrentPage:int}}";

    public const string CoursesDetailsBase = "/courses/details";
    public const string CoursesDetailsCourseId = $"{CoursesDetailsBase}/{{CourseId:int}}";
    public const string CoursesCourseId = "/courses/{CourseId:int}";
    public const string CoursesTag = "/courses-tag";
    public const string CoursesTagPageCurrentPage = $"{CoursesTag}/page/{{CurrentPage:int}}";
    public const string CoursesTagTagName = $"{CoursesTag}/{{TagName}}";
    public const string CoursesTagTagNamePageCurrentPage = $"{CoursesTag}/{{TagName}}/page/{{CurrentPage:int}}";
    public const string CoursesWriters = "/courses-writers";
    public const string CoursesWritersPageCurrentPage = "/courses-writers/page/{CurrentPage:int}";
    public const string CoursesWritersUserFriendlyName = "/courses-writers/{UserFriendlyName}";

    public const string CoursesWritersUserFriendlyNamePageCurrentPage =
        "/courses-writers/{UserFriendlyName}/page/{CurrentPage:int}";

    public const string CoursesTopicBase = "/courses/topic";
    public const string CoursesTopicCourseIdDisplayId = $"{CoursesTopicBase}/{{CourseId:int}}/{{DisplayId:guid}}";
    public const string CoursesComments = "/courses-comments";
    public const string CoursesCommentsPageCurrentPage = $"{CoursesComments}/page/{{CurrentPage:int}}";
    public const string CoursesCommentsUserFriendlyName = $"{CoursesComments}/{{UserFriendlyName}}";

    public const string CoursesCommentsUserFriendlyNamePageCurrentPage =
        $"{CoursesComments}/{{UserFriendlyName}}/page/{{CurrentPage:int}}";

    public const string CourseCommentsBase = "/course-comments";
    public const string CourseCommentsCourseId = $"{CourseCommentsBase}/{{CourseId:int}}";

    public const string CourseCommentsCourseIdPageCurrentPage =
        $"{CourseCommentsBase}/{{CourseId:int}}/page/{{CurrentPage:int}}";

    public const string WriteCourse = "/write-course";
    public const string WriteCourseEditEditId = $"{WriteCourse}/edit/{{EditId:{EncryptedRouteConstraint.Name}}}";

    public const string WriteCourseDeleteDeleteId =
        $"{WriteCourse}/delete/{{DeleteId:{EncryptedRouteConstraint.Name}}}";

    public const string EditPostUrlTemplate = $"{WriteCourse}/edit/{{0}}";
    public const string DeletePostUrlTemplate = $"{WriteCourse}/delete/{{0}}";

    public const string WriteCourseTopicBase = "/write-course-topic";
    public const string WriteCourseTopicCourseId = $"{WriteCourseTopicBase}/{{CourseId:int}}";

    public const string WriteCourseTopicEditBase = $"{WriteCourseTopicBase}/edit";

    public const string WriteCourseTopicEditCourseIdEditId =
        $"{WriteCourseTopicEditBase}/{{CourseId:int}}/{{EditId:{EncryptedRouteConstraint.Name}}}";

    public const string WriteCourseTopicDeleteBase = $"{WriteCourseTopicBase}/delete";

    public const string WriteCourseTopicDeleteCourseIdDeleteId =
        $"{WriteCourseTopicDeleteBase}/{{CourseId:int}}/{{DeleteId:{EncryptedRouteConstraint.Name}}}";

    public const string CommentsUrlTemplate = $"{CourseCommentsBase}/{{0}}";
    public const string PostUrlTemplate = $"{CoursesDetailsBase}/{{0}}";
    public const string PostTagUrlTemplate = $"{CoursesTag}/{{0}}";
}
