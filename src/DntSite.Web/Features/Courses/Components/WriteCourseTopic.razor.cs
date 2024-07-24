using AutoMapper;
using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;
using DntSite.Web.Features.Courses.RoutingConstants;
using DntSite.Web.Features.Courses.Services.Contracts;

namespace DntSite.Web.Features.Courses.Components;

[Authorize]
public partial class WriteCourseTopic
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [SupplyParameterFromForm(FormName = nameof(WriteCourseTopic))]
    public CourseTopicItemModel WriteCourseItemModel { get; set; } = new();

    [Parameter] public int? CourseId { set; get; }

    [Parameter] public Guid? EditId { set; get; }

    [Parameter] public Guid? DeleteId { set; get; }

    [InjectComponentScoped] internal ICourseTopicsService CourseTopicsService { set; get; } = null!;

    [InjectComponentScoped] internal ICoursesService CoursesService { set; get; } = null!;

    [Inject] internal IMapper Mapper { set; get; } = null!;

    private async Task<string?> GetCourseTitleAsync()
        => CourseId.HasValue ? (await CoursesService.FindCourseAsync(CourseId.Value))?.Title : null;

    protected override async Task OnInitializedAsync()
    {
        if (!CourseId.HasValue ||
            !await CourseTopicsService.CanUserAddCourseTopicAsync(ApplicationState.CurrentUser, CourseId.Value))
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        AddBreadCrumbs(await GetCourseTitleAsync());

        if (!ApplicationState.HttpContext.IsGetRequest())
        {
            return;
        }

        await PerformPossibleDeleteAsync();

        await FillPossibleEditFormAsync();
    }

    private async Task PerformPossibleDeleteAsync()
    {
        if (!DeleteId.HasValue)
        {
            return;
        }

        var courseTopic = await GetCourseTopicItemAsync(DeleteId.Value);
        await CourseTopicsService.MarkAsDeletedAsync(courseTopic);
        await CourseTopicsService.NotifyAddOrUpdateChangesAsync(courseTopic);

        NavigateToMainCoursePage(courseTopic);
    }

    private void NavigateToMainCoursePage(CourseTopic? courseTopic)
        => ApplicationState.NavigateTo(
            Invariant($"{CoursesRoutingConstants.CoursesDetailsBase}/{courseTopic?.CourseId}"));

    private void AddBreadCrumbs(string? courseTitle)
        => ApplicationState.BreadCrumbs.AddRange([
            ..CoursesBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
            {
                Title = $"دوره «{courseTitle}»",
                Url = Invariant($"{CoursesRoutingConstants.Courses}/{CourseId}"),
                GlyphIcon = DntBootstrapIcons.BiMortarboard
            },
            new BreadCrumb
            {
                Title = "تعریف مطلب دوره",
                Url = Invariant($"{CoursesRoutingConstants.WriteCourseTopicBase}/{CourseId}"),
                GlyphIcon = DntBootstrapIcons.BiPencil
            }
        ]);

    private async Task FillPossibleEditFormAsync()
    {
        if (!EditId.HasValue)
        {
            return;
        }

        var item = await GetCourseTopicItemAsync(EditId.Value);

        if (item is null)
        {
            return;
        }

        WriteCourseItemModel = Mapper.Map<CourseTopic, CourseTopicItemModel>(item);
    }

    private async Task<CourseTopic?> GetCourseTopicItemAsync(Guid displayId)
    {
        var item = await CourseTopicsService.FindCourseTopicAsync(displayId);

        if (item is null || !ApplicationState.CanCurrentUserEditThisItem(item.UserId, item.Audit.CreatedAt))
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return null;
        }

        return item;
    }

    private async Task PerformAsync()
    {
        if (!CourseId.HasValue)
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        var user = ApplicationState.CurrentUser?.User;

        CourseTopic? courseTopic;

        if (EditId.HasValue)
        {
            courseTopic = await GetCourseTopicItemAsync(EditId.Value);
            await CourseTopicsService.UpdateCourseTopicItemAsync(courseTopic, WriteCourseItemModel);
        }
        else
        {
            courseTopic = await CourseTopicsService.AddCourseTopicItemAsync(WriteCourseItemModel, user, CourseId.Value);
        }

        await CourseTopicsService.NotifyAddOrUpdateChangesAsync(courseTopic);
        NavigateToMainCoursePage(courseTopic);
    }
}
