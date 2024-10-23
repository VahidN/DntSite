namespace DntSite.Web.Features.Projects.RoutingConstants;

public static class ProjectsRoutingConstants
{
    public const string Projects = "/projects";
    public const string ProjectsPageCurrentPage = $"{Projects}/page/{{CurrentPage:int?}}";

    public const string ProjectsFilterBase = $"{Projects}/filter";

    public const string ProjectsFilterFilterPageCurrentPage =
        $"{ProjectsFilterBase}/{{Filter}}/page/{{CurrentPage:int?}}";

    public const string ProjectsDetailsBase = "/project/details";
    public const string ProjectsDetailsProjectId = $"{ProjectsDetailsBase}/{{ProjectId:int}}";

    public const string ProjectsDetailsOldProjectId = "/projects/details/{ProjectId:int}";

    public const string ProjectsTag = "/projects-tag";
    public const string ProjectsTagPageCurrentPage = $"{ProjectsTag}/page/{{CurrentPage:int?}}";
    public const string ProjectsTagTagName = $"{ProjectsTag}/{{TagName}}";
    public const string ProjectsTagTagNamePageCurrentPage = $"{ProjectsTag}/{{TagName}}/page/{{CurrentPage:int?}}";

    public const string ProjectsWriters = "/projects-writers";
    public const string ProjectsWritersPageCurrentPage = $"{ProjectsWriters}/page/{{CurrentPage:int?}}";
    public const string ProjectsWritersUserFriendlyName = $"{ProjectsWriters}/{{UserFriendlyName}}";

    public const string ProjectsWritersUserFriendlyNamePageCurrentPage =
        $"{ProjectsWriters}/{{UserFriendlyName}}/page/{{CurrentPage:int?}}";

    public const string ProjectsComments = "/projects-comments";
    public const string ProjectsCommentsPageCurrentPage = $"{ProjectsComments}/page/{{CurrentPage:int?}}";
    public const string ProjectsCommentsUserFriendlyName = $"{ProjectsComments}/{{UserFriendlyName}}";

    public const string ProjectsCommentsUserFriendlyNamePageCurrentPage =
        $"{ProjectsComments}/{{UserFriendlyName}}/page/{{CurrentPage:int?}}";

    public const string ProjectCommentsBase = "/project-comments";
    public const string ProjectCommentsProjectId = $"{ProjectCommentsBase}/{{ProjectId:int}}";

    public const string ProjectCommentsProjectIdPageCurrentPage =
        $"{ProjectCommentsBase}/{{ProjectId:int}}/page/{{CurrentPage:int?}}";

    public const string WriteProject = "/write-project";

    public const string WriteProjectEditBase = $"{WriteProject}/edit";
    public const string WriteProjectEditEditId = $"{WriteProjectEditBase}/{{EditId:{EncryptedRouteConstraint.Name}}}";

    public const string WriteProjectDeleteBase = $"{WriteProject}/delete";

    public const string WriteProjectDeleteDeleteId =
        $"{WriteProjectDeleteBase}/{{DeleteId:{EncryptedRouteConstraint.Name}}}";

    public const string FeedbacksUrlTemplate = $"{ProjectFeedbacksBase}/{{0}}#comments";
    public const string CommentsUrlTemplate = $"{ProjectCommentsBase}/{{0}}#comments";
    public const string PostUrlTemplate = $"{ProjectsDetailsBase}/{{0}}";
    public const string PostTagUrlTemplate = $"{ProjectsTag}/{{0}}";
    public const string EditPostUrlTemplate = $"{WriteProjectEditBase}/{{0}}";
    public const string DeletePostUrlTemplate = $"{WriteProjectDeleteBase}/{{0}}";

    public const string ProjectsFeedbacks = "/projects-feedbacks";
    public const string ProjectsFeedbacksPageCurrentPage = $"{ProjectsFeedbacks}/page/{{CurrentPage:int?}}";
    public const string ProjectsFeedbacksUserFriendlyName = $"{ProjectsFeedbacks}/{{UserFriendlyName}}";

    public const string ProjectsFeedbacksUserFriendlyNamePageCurrentPage =
        $"{ProjectsFeedbacks}/{{UserFriendlyName}}/page/{{CurrentPage:int?}}";

    public const string ProjectFeedbacksBase = "/project-feedbacks";
    public const string ProjectFeedbacksProjectId = $"{ProjectFeedbacksBase}/{{ProjectId:int?}}";

    public const string ProjectFeedbacksOldProjectId = "/projectissues/project/{ProjectId:int}";

    public const string ProjectFeedbacksProjectIdPageCurrentPage =
        $"{ProjectFeedbacksBase}/{{ProjectId:int}}/page/{{CurrentPage:int?}}";

    public const string ProjectFeedbacksProjectIdFeedbackId =
        $"{ProjectFeedbacksBase}/{{ProjectId:int}}/{{FeedbackId:int}}";

    public const string ProjectFeedbacksOldProjectIdFeedbackId =
        "/projectissue/details/{ProjectId:int}/{FeedbackId:int}";

    public const string WriteProjectFaqBase = "/write-project-faq";
    public const string WriteProjectFaq = $"{WriteProjectFaqBase}/{{ProjectId:int}}";

    public const string WriteProjectFaqEditBase = $"{WriteProjectFaqBase}/edit";

    public const string WriteProjectFaqEditEditId =
        $"{WriteProjectFaqEditBase}/{{ProjectId:int}}/{{EditId:{EncryptedRouteConstraint.Name}}}";

    public const string WriteProjectFaqDeleteBase = $"{WriteProjectFaqBase}/delete";

    public const string WriteProjectFaqDeleteDeleteId =
        $"{WriteProjectFaqDeleteBase}/{{ProjectId:int}}/{{DeleteId:{EncryptedRouteConstraint.Name}}}";

    public const string EditProjectFaqUrlTemplate = $"{WriteProjectFaqEditBase}/{{0}}";
    public const string DeleteProjectFaqUrlTemplate = $"{WriteProjectFaqDeleteBase}/{{0}}";

    public const string WriteProjectFeedbackBase = "/write-project-feedback";
    public const string WriteProjectFeedback = $"{WriteProjectFeedbackBase}/{{ProjectId:int}}";

    public const string WriteProjectFeedbackEditBase = $"{WriteProjectFeedbackBase}/edit";

    public const string WriteProjectFeedbackEditEditId =
        $"{WriteProjectFeedbackEditBase}/{{ProjectId:int}}/{{EditId:{EncryptedRouteConstraint.Name}}}";

    public const string WriteProjectFeedbackDeleteBase = $"{WriteProjectFeedbackBase}/delete";

    public const string WriteProjectFeedbackDeleteDeleteId =
        $"{WriteProjectFeedbackDeleteBase}/{{ProjectId:int}}/{{DeleteId:{EncryptedRouteConstraint.Name}}}";

    public const string WriteProjectReleaseBase = "/write-project-release";
    public const string WriteProjectRelease = $"{WriteProjectReleaseBase}/{{ProjectId:int}}";

    public const string WriteProjectReleaseEditBase = $"{WriteProjectReleaseBase}/edit";

    public const string WriteProjectReleaseEditEditId =
        $"{WriteProjectReleaseEditBase}/{{ProjectId:int}}/{{EditId:{EncryptedRouteConstraint.Name}}}";

    public const string WriteProjectReleaseDeleteBase = $"{WriteProjectReleaseBase}/delete";

    public const string WriteProjectReleaseDeleteDeleteId =
        $"{WriteProjectReleaseDeleteBase}/{{ProjectId:int}}/{{DeleteId:{EncryptedRouteConstraint.Name}}}";

    public const string EditProjectReleaseUrlTemplate = $"{WriteProjectReleaseEditBase}/{{0}}";
    public const string DeleteProjectReleaseUrlTemplate = $"{WriteProjectReleaseDeleteBase}/{{0}}";

    public const string ProjectsFaqs = "/projects-faqs";
    public const string ProjectsFaqsPageCurrentPage = $"{ProjectsFaqs}/page/{{CurrentPage:int?}}";
    public const string ProjectsFaqsUserFriendlyName = $"{ProjectsFaqs}/{{UserFriendlyName}}";

    public const string ProjectsFaqsUserFriendlyNamePageCurrentPage =
        $"{ProjectsFaqs}/{{UserFriendlyName}}/page/{{CurrentPage:int?}}";

    public const string ProjectFaqsBase = "/project-faqs";
    public const string ProjectFaqsProjectId = $"{ProjectFaqsBase}/{{ProjectId:int}}";

    public const string ProjectFaqsProjectIdPageCurrentPage =
        $"{ProjectFaqsBase}/{{ProjectId:int}}/page/{{CurrentPage:int?}}";

    public const string ProjectFaqsProjectIdFaqId = $"{ProjectFaqsBase}/{{ProjectId:int}}/{{FaqId:int}}";

    public const string ProjectFaqsOldProjectIdFaqId = "/projectfaq/faq/{ProjectId:int}/{FaqId:int}";

    public const string ProjectsReleases = "/projects-releases";
    public const string ProjectsReleasesPageCurrentPage = $"{ProjectsReleases}/page/{{CurrentPage:int?}}";
    public const string ProjectsReleasesUserFriendlyName = $"{ProjectsReleases}/{{UserFriendlyName}}";

    public const string ProjectsReleasesUserFriendlyNamePageCurrentPage =
        $"{ProjectsReleases}/{{UserFriendlyName}}/page/{{CurrentPage:int?}}";

    public const string ProjectReleasesBase = "/project-releases";
    public const string ProjectReleasesProjectId = $"{ProjectReleasesBase}/{{ProjectId:int}}";

    public const string ProjectReleasesProjectIdPageCurrentPage =
        $"{ProjectReleasesBase}/{{ProjectId:int}}/page/{{CurrentPage:int?}}";

    public const string ProjectReleasesProjectIdReleaseId =
        $"{ProjectReleasesBase}/{{ProjectId:int}}/{{ReleaseId:int}}";
}
