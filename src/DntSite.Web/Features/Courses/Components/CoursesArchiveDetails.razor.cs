using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;
using DntSite.Web.Features.Courses.RoutingConstants;
using DntSite.Web.Features.Courses.Services.Contracts;

namespace DntSite.Web.Features.Courses.Components;

public partial class CoursesArchiveDetails
{
    private CourseItemModel? _courseModel;

    [Parameter] public int? CourseId { set; get; }

    private Course? CurrentPost => _courseModel?.CurrentCourse;

    private DateTime? ModifiedAt => CurrentPost?.AuditActions.Count > 0
        ? CurrentPost?.AuditActions[^1].CreatedAt
        : CurrentPost?.Audit.CreatedAt;

    private string? CurrentPostImageUrl
        => HtmlHelperService.ExtractImagesLinks(CurrentPost?.Description ?? "").FirstOrDefault();

    [InjectComponentScoped] internal ICoursesService CoursesService { set; get; } = null!;

    [Inject] internal IHtmlHelperService HtmlHelperService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public string? Slug { set; get; }

    private bool CanUserDeleteThisPost => ApplicationState.IsCurrentUserAdmin;

    private bool CanUserEditThisPost => ApplicationState.CanCurrentUserEditThisItem(CurrentPost?.UserId);

    private bool CanUserSeeThisItem(Course currentCourse)
        => currentCourse.IsReadyToPublish ||
           ApplicationState.CanCurrentUserEditThisItem(currentCourse.UserId, currentCourse.Audit.CreatedAt);

    private List<string> GetTags() => CurrentPost?.Tags.Select(x => x.Name).ToList() ?? [];

    protected override async Task OnInitializedAsync()
    {
        AddBreadCrumbs();

        if (!CourseId.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _courseModel = await CoursesService.GetCurrentCourseLastAndNextAsync(CourseId.Value, showOnlyFinished: false);

        if (_courseModel.CurrentCourse is null)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        if (!CanUserSeeThisItem(_courseModel.CurrentCourse))
        {
            ApplicationState.NavigateToUnauthorizedPage();
        }
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..CoursesBreadCrumbs.DefaultBreadCrumbs]);
}
