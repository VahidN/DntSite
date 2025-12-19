using AutoMapper;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;
using DntSite.Web.Features.Courses.RoutingConstants;
using DntSite.Web.Features.Courses.Services.Contracts;

namespace DntSite.Web.Features.Courses.Components;

[Authorize]
public partial class WriteCourse
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    public IList<string>? AutoCompleteDataList { get; set; }

    [SupplyParameterFromForm(FormName = nameof(WriteCourse))]
    public CourseModel? WriteCourseModel { get; set; }

    [InjectComponentScoped] internal ITagsService TagsService { set; get; } = null!;

    [Parameter] public string? EditId { set; get; }

    [Parameter] public string? DeleteId { set; get; }

    [InjectComponentScoped] internal ICoursesService CoursesService { set; get; } = null!;

    [Inject] internal IMapper Mapper { set; get; } = null!;

    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    public IReadOnlyDictionary<string, string> CourseLevelItems { set; get; } =
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            {
                "مقدماتی", "مقدماتی"
            },
            {
                "متوسط", "متوسط"
            },
            {
                "پیشرفته", "پیشرفته"
            }
        };

    protected override async Task OnInitializedAsync()
    {
        WriteCourseModel ??= new CourseModel();

        if (!ApplicationState.CanCurrentUserCreateANewCourse())
        {
            Alert.ShowAlert(AlertType.Danger, title: "عدم دسترسی",
                message: "برای ایجاد یک دوره جدید نیاز است حداقل دو مطلب ارسالی در سایت داشته باشید.");

            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        AutoCompleteDataList = await TagsService.GetTagNamesArrayAsync(count: 2000);
        AddBreadCrumbs();

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

        var course = await GetCourseItemAsync(DeleteId.ToInt());
        await CoursesService.MarkAsDeletedAsync(course);
        await CoursesService.NotifyDeleteChangesAsync(course);

        ApplicationState.NavigateTo(CoursesRoutingConstants.Courses);
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..CoursesBreadCrumbs.DefaultBreadCrumbs, CoursesBreadCrumbs.WriteCourse
        ]);

    private async Task FillPossibleEditFormAsync()
    {
        if (string.IsNullOrWhiteSpace(EditId))
        {
            return;
        }

        var item = await GetCourseItemAsync(EditId.ToInt());

        if (item is null)
        {
            return;
        }

        WriteCourseModel = Mapper.Map<Course, CourseModel>(item);
    }

    private async Task<Course?> GetCourseItemAsync(int id)
    {
        var item = await CoursesService.FindCourseIncludeTagsAndUserAsync(id);

        if (item is null || !ApplicationState.CanCurrentUserEditThisItem(item.UserId, item.Audit.CreatedAt))
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return null;
        }

        return item;
    }

    private async Task PerformAsync()
    {
        var user = ApplicationState.CurrentUser?.User;

        Course? course;

        if (!string.IsNullOrWhiteSpace(EditId))
        {
            course = await GetCourseItemAsync(EditId.ToInt());
            await CoursesService.UpdateCourseItemAsync(course, WriteCourseModel);
        }
        else
        {
            course = await CoursesService.AddCourseItemAsync(WriteCourseModel, user);
        }

        await CoursesService.NotifyAddOrUpdateChangesAsync(course, WriteCourseModel, user);

        ApplicationState.NavigateTo(string.Create(CultureInfo.InvariantCulture,
            $"{CoursesRoutingConstants.CoursesDetailsBase}/{course?.Id}"));
    }
}
