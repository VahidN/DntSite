using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.RoutingConstants;
using DntSite.Web.Features.Courses.Services.Contracts;

namespace DntSite.Web.Features.Courses.Components;

public partial class ShowCoursesComments
{
    private const int PostItemsPerPage = 10;

    private string _pageTitle = "نظرات دوره‌ها";

    private PagedResultModel<CourseTopicComment>? _posts;

    [MemberNotNullWhen(returnValue: true, nameof(UserFriendlyName))]
    private bool HasUserFriendlyName => !string.IsNullOrWhiteSpace(UserFriendlyName);

    [Parameter] public string? UserFriendlyName { set; get; }

    [Parameter] public int? CourseId { set; get; }

    [InjectComponentScoped] internal ICourseTopicCommentsService CourseTopicCommentsService { set; get; } = null!;

    [InjectComponentScoped] internal ICoursesService CoursesService { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private async Task<string> GetMainTitleAsync()
    {
        if (HasUserFriendlyName)
        {
            return $@"آرشیو نظرات دوره‌های {UserFriendlyName}";
        }

        if (CourseId.HasValue)
        {
            var courseName = (await CoursesService.FindCourseAsync(CourseId.Value))?.Title;

            return $"نظرات دوره {courseName}";
        }

        return "نظرات دوره‌ها";
    }

    private string GetBasePath()
    {
        if (HasUserFriendlyName)
        {
            return $"{CoursesRoutingConstants.CoursesComments}/{Uri.EscapeDataString(UserFriendlyName)}";
        }

        return CourseId.HasValue
            ? string.Create(CultureInfo.InvariantCulture,
                $"{CoursesRoutingConstants.CourseCommentsBase}/{CourseId.Value}")
            : CoursesRoutingConstants.CoursesComments;
    }

    protected override async Task OnInitializedAsync()
    {
        _pageTitle = await GetMainTitleAsync();

        if (HasUserFriendlyName)
        {
            await ShowUserCommentsAsync();
        }
        else if (CourseId.HasValue)
        {
            await ShowCourseCommentsListAsync(CourseId.Value);
        }
        else
        {
            await ShowAllCoursesCommentsListAsync();
        }
    }

    private async Task ShowCourseCommentsListAsync(int courseId)
    {
        CurrentPage ??= 1;

        _posts = await CourseTopicCommentsService.GetLastPagedCommentsOfCourseAsNoTrackingAsync(courseId,
            CurrentPage.Value - 1, PostItemsPerPage);

        AddUserCommentsBreadCrumbs();
    }

    private async Task ShowAllCoursesCommentsListAsync()
    {
        CurrentPage ??= 1;

        _posts = await CourseTopicCommentsService.GetLastPagedCommentsAsNoTrackingAsync(CurrentPage.Value - 1,
            PostItemsPerPage);

        AddCommentsListBreadCrumbs();
    }

    private void AddCommentsListBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([..CoursesBreadCrumbs.DefaultBreadCrumbs]);

    private async Task ShowUserCommentsAsync()
    {
        CurrentPage ??= 1;

        _posts = await CourseTopicCommentsService.GetLastPagedCommentsAsNoTrackingAsync(UserFriendlyName!,
            CurrentPage.Value - 1, PostItemsPerPage);

        AddUserCommentsBreadCrumbs();
    }

    private void AddUserCommentsBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..CoursesBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
            {
                Title = _pageTitle,
                Url = GetBasePath(),
                GlyphIcon = DntBootstrapIcons.BiPerson
            }
        ]);

    private string GetPostAbsoluteUrl(CourseTopicComment comment)
        => string.Create(CultureInfo.InvariantCulture,
            $"{CoursesRoutingConstants.CoursesTopicBase}/{comment.Parent.CourseId}/{comment.Parent.DisplayId.ToString(format: "D")}");
}
