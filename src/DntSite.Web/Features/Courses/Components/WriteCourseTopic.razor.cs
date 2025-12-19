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
    public CourseTopicItemModel? WriteCourseItemModel { get; set; }

    [Parameter] public int? CourseId { set; get; }

    [Parameter] public string? EditId { set; get; }

    [Parameter] public string? DeleteId { set; get; }

    [InjectComponentScoped] internal ICourseTopicsService CourseTopicsService { set; get; } = null!;

    [InjectComponentScoped] internal ICoursesService CoursesService { set; get; } = null!;

    [Inject] internal IMapper Mapper { set; get; } = null!;

    private async Task<string?> GetCourseTitleAsync()
        => CourseId.HasValue ? (await CoursesService.FindCourseAsync(CourseId.Value))?.Title : null;

    protected override async Task OnInitializedAsync()
    {
        WriteCourseItemModel ??= new CourseTopicItemModel();

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
        if (string.IsNullOrWhiteSpace(DeleteId))
        {
            return;
        }

        var courseTopic = await GetCourseTopicItemAsync(new Guid(DeleteId));
        await CourseTopicsService.MarkAsDeletedAsync(courseTopic);
        await CourseTopicsService.NotifyAddOrUpdateChangesAsync(courseTopic);

        NavigateToMainCoursePage(courseTopic);
    }

    private void NavigateToMainCoursePage(CourseTopic? courseTopic)
        => ApplicationState.NavigateTo(string.Create(CultureInfo.InvariantCulture,
            $"{CoursesRoutingConstants.CoursesDetailsBase}/{courseTopic?.CourseId}"));

    private void AddBreadCrumbs(string? courseTitle)
        => ApplicationState.BreadCrumbs.AddRange([
            ..CoursesBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
            {
                Title = $"دوره «{courseTitle}»",
                Url = string.Create(CultureInfo.InvariantCulture, $"{CoursesRoutingConstants.Courses}/{CourseId}"),
                GlyphIcon = DntBootstrapIcons.BiMortarboard
            },
            new BreadCrumb
            {
                Title = "تعریف مطلب دوره",
                Url = string.Create(CultureInfo.InvariantCulture,
                    $"{CoursesRoutingConstants.WriteCourseTopicBase}/{CourseId}"),
                GlyphIcon = DntBootstrapIcons.BiPencil
            }
        ]);

    private async Task FillPossibleEditFormAsync()
    {
        if (string.IsNullOrWhiteSpace(EditId))
        {
            return;
        }

        var item = await GetCourseTopicItemAsync(new Guid(EditId));

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

        if (!string.IsNullOrWhiteSpace(EditId))
        {
            courseTopic = await GetCourseTopicItemAsync(new Guid(EditId));
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
