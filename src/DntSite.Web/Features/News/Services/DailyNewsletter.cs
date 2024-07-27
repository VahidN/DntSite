using System.Text;
using DntSite.Web.Features.Backlogs.RoutingConstants;
using DntSite.Web.Features.Backlogs.Services.Contracts;
using DntSite.Web.Features.Common.Models;
using DntSite.Web.Features.Courses.RoutingConstants;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.News.RoutingConstants;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Posts.RoutingConstants;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.Projects.Services.Contracts;
using DntSite.Web.Features.RoadMaps.RoutingConstants;
using DntSite.Web.Features.RoadMaps.Services.Contracts;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

namespace DntSite.Web.Features.News.Services;

public class DailyNewsletter(
    IBlogPostsService blogPostsService,
    IDailyNewsItemsService newsItemsService,
    ICourseTopicsService courseTopicsService,
    IProjectsService projectsService,
    IBacklogsService backlogsService,
    ILearningPathService learningPathService,
    IQuestionsService stackExchangeService) : IDailyNewsletter
{
    private const string Hr = "<hr style='border:none;border-bottom:solid #EEEEFF 1.0pt;padding:0'>";
    private readonly StringBuilder _data = new();

    // It runs in a http context less environment.
    public async Task<string> GetEmailContentAsync(string url, DateTime yesterday)
    {
        await CreatePostsAsync(yesterday, url);
        await CreateCoursesNewsAsync(yesterday, url);
        await CreateNewsAsync(yesterday, url);
        await CreateProjectsAsync(yesterday, url);
        await CreateBacklogsAsync(yesterday, url);
        await CreateLearningPathsAsync(yesterday, url);
        await CreateStackExchangeItemsAsync(url);

        return _data.ToString();
    }

    private static string WrapContentWithCorrectDirection(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            return string.Empty;
        }

        return !data.ContainsFarsi() ? $"<div align='left' dir='ltr'>{data}</div>" : data;
    }

    private async Task CreateBacklogsAsync(DateTime yesterday, string url)
    {
        var posts = await backlogsService.GetAllPublicBacklogsOfDateAsync(yesterday);

        if (posts.Count == 0)
        {
            posts = (await backlogsService.GetBacklogsAsync(pageNumber: 0, isNewItems: true)).Data
                .OrderBy(_ => Guid.NewGuid())
                .Take(count: 1)
                .ToList();
        }

        if (posts.Count == 0)
        {
            return;
        }

        _data.AppendFormat(CultureInfo.InvariantCulture, format: "<br><br>{0}<br>", Hr);
        _data.Append(value: "<b>آخرین پیشنهاد‌های ارائه مطلب: </b><br/>");
        _data.Append(value: "<ul>");

        foreach (var post in posts)
        {
            _data.Append(value: "<li style='margin-top:12px'>");

            var finalUrl = url.CombineUrl(BacklogsRoutingConstants.BacklogsDetailsBase)
                .CombineUrl(post.Id.ToString(CultureInfo.InvariantCulture));

            _data.AppendFormat(CultureInfo.InvariantCulture, format: "<a dir='{2}' href='{0}'><b>{1}</b></a><br/>",
                finalUrl, post.Title, post.Title.GetDir());

            _data.Append(post.User!.FriendlyName);
            _data.AppendFormat(CultureInfo.InvariantCulture, format: "{0}<br/>", Hr);
            _data.Append(WrapContentWithCorrectDirection(post.Description.GetBriefDescription(charLength: 200)));
            _data.Append(value: "<br/><br/>");
            _data.Append(value: "</li>");
        }

        _data.Append(value: "</ul>");
    }

    private async Task CreateCoursesNewsAsync(DateTime yesterday, string url)
    {
        var posts = await courseTopicsService.GetAllPublicTopicsOfDateAsync(yesterday);

        if (posts.Count == 0)
        {
            return;
        }

        _data.AppendFormat(CultureInfo.InvariantCulture, format: "<br><br>{0}<br>", Hr);
        _data.Append(value: "<b>آخرین مطالب دوره‌ها: </b><br/>");
        _data.Append(value: "<ul>");

        foreach (var post in posts)
        {
            _data.Append(value: "<li style='margin-top:12px'>");

            var finalUrl = url.CombineUrl(CoursesRoutingConstants.CoursesTopicBase)
                .CombineUrl(post.CourseId.ToString(CultureInfo.InvariantCulture))
                .CombineUrl(post.DisplayId.ToString(format: "D"));

            _data.AppendFormat(CultureInfo.InvariantCulture, format: "<a dir='{2}' href='{0}'><b>{1}</b></a><br/>",
                finalUrl, post.Title, post.Title.GetDir());

            _data.Append(post.User!.FriendlyName);
            _data.AppendFormat(CultureInfo.InvariantCulture, format: "{0}<br/>", Hr);
            _data.Append(WrapContentWithCorrectDirection(post.Body.GetBriefDescription(charLength: 200)));
            _data.Append(value: "<br/><br/>");
            _data.Append(value: "</li>");
        }

        _data.Append(value: "</ul>");
    }

    private async Task CreateLearningPathsAsync(DateTime yesterday, string url)
    {
        var posts = await learningPathService.GetAllPublicLearningPathsOfDateAsync(yesterday);

        if (posts.Count == 0)
        {
            return;
        }

        _data.AppendFormat(CultureInfo.InvariantCulture, format: "<br><br>{0}<br>", Hr);
        _data.Append(value: "<b>آخرین نقشه‌های راه: </b><br/>");
        _data.Append(value: "<ul>");

        foreach (var post in posts)
        {
            _data.Append(value: "<li style='margin-top:12px'>");

            var finalUrl = url.CombineUrl(RoadMapsRoutingConstants.LearningPathsDetailsBase)
                .CombineUrl(post.Id.ToString(CultureInfo.InvariantCulture));

            _data.AppendFormat(CultureInfo.InvariantCulture, format: "<a dir='{2}' href='{0}'><b>{1}</b></a><br/>",
                finalUrl, post.Title, post.Title.GetDir());

            _data.Append(post.User!.FriendlyName);
            _data.AppendFormat(CultureInfo.InvariantCulture, format: "{0}<br/>", Hr);
            _data.Append(WrapContentWithCorrectDirection(post.Description.GetBriefDescription(charLength: 200)));
            _data.Append(value: "<br/><br/>");
            _data.Append(value: "</li>");
        }

        _data.Append(value: "</ul>");
    }

    private async Task CreateNewsAsync(DateTime yesterday, string url)
    {
        var news = await newsItemsService.GetAllPublicNewsOfDateAsync(yesterday);

        if (news.Count == 0)
        {
            return;
        }

        _data.AppendFormat(CultureInfo.InvariantCulture, format: "<br><br>{0}<br>", Hr);
        _data.Append(value: "<b>آخرین اشتراک‌ها: </b><br/>");
        _data.Append(value: "<ul>");

        foreach (var post in news)
        {
            _data.Append(value: "<li style='margin-top:12px'>");

            var finalUrl = url.CombineUrl(NewsRoutingConstants.NewsDetailsBase)
                .CombineUrl(post.Id.ToString(CultureInfo.InvariantCulture));

            _data.AppendFormat(CultureInfo.InvariantCulture, format: "<a dir='{2}' href='{0}'><b>{1}</b></a><br/>",
                finalUrl, post.Title, post.Title.GetDir());

            _data.Append(post.User!.FriendlyName);
            _data.Append(value: "<br/><br/>");

            if (!string.IsNullOrWhiteSpace(post.BriefDescription))
            {
                _data.Append(WrapContentWithCorrectDirection(post.BriefDescription));
                _data.Append(value: "<br/>");
            }

            _data.Append(value: "</li>");
        }

        _data.Append(value: "</ul>");
    }

    private async Task CreatePostsAsync(DateTime yesterday, string url)
    {
        var posts = await blogPostsService.GetAllPublicPostsOfDateAsync(yesterday);

        if (posts.Count == 0)
        {
            return;
        }

        _data.Append(value: "<b>آخرین مطالب ارسالی: </b><br/>");
        _data.Append(value: "<ul>");

        foreach (var post in posts)
        {
            _data.Append(value: "<li style='margin-top:12px'>");

            var finalUrl = url.CombineUrl(PostsRoutingConstants.PostBase)
                .CombineUrl(post.Id.ToString(CultureInfo.InvariantCulture));

            _data.AppendFormat(CultureInfo.InvariantCulture, format: "<a dir='{2}' href='{0}'><b>{1}</b></a><br/>",
                finalUrl, post.Title, post.Title.GetDir());

            _data.Append(post.User!.FriendlyName);
            _data.AppendFormat(CultureInfo.InvariantCulture, format: "{0}<br/>", Hr);
            _data.Append(WrapContentWithCorrectDirection(post.Body));
            _data.Append(value: "<br/><br/>");
            _data.Append(value: "</li>");
        }

        _data.Append(value: "</ul>");
    }

    private async Task CreateProjectsAsync(DateTime yesterday, string url)
    {
        var projects = await projectsService.GetAllPublicProjectsOfDateAsync(yesterday);

        if (projects.Count == 0)
        {
            return;
        }

        _data.AppendFormat(CultureInfo.InvariantCulture, format: "<br><br>{0}<br>", Hr);
        _data.Append(value: "<b>آخرین پروژه‌ها: </b><br/>");
        _data.Append(value: "<ul>");

        foreach (var post in projects)
        {
            _data.Append(value: "<li style='margin-top:12px'>");

            var finalUrl = url.CombineUrl(ProjectsRoutingConstants.ProjectsDetailsBase)
                .CombineUrl(post.Id.ToString(CultureInfo.InvariantCulture));

            _data.AppendFormat(CultureInfo.InvariantCulture, format: "<a dir='{2}' href='{0}'><b>{1}</b></a><br/>",
                finalUrl, post.Title, post.Title.GetDir());

            _data.Append(post.User!.FriendlyName);
            _data.AppendFormat(CultureInfo.InvariantCulture, format: "{0}<br/>", Hr);
            _data.Append(WrapContentWithCorrectDirection(post.Description.GetBriefDescription(charLength: 200)));
            _data.Append(value: "<br/><br/>");
            _data.Append(value: "</li>");
        }

        _data.Append(value: "</ul>");
    }

    private async Task CreateStackExchangeItemsAsync(string url)
    {
        var questions =
            (await stackExchangeService.GetStackExchangeQuestionsAsync(pageNumber: 0, isNewItems: true,
                recordsPerPage: 5)).Data;

        if (questions.Count == 0)
        {
            return;
        }

        _data.AppendFormat(CultureInfo.InvariantCulture, format: "<br><br>{0}<br>", Hr);
        _data.Append(value: "<b>آخرین پرسش‌ها: </b><br/>");
        _data.Append(value: "<ul>");

        foreach (var question in questions)
        {
            if (string.IsNullOrWhiteSpace(question.Description))
            {
                continue;
            }

            _data.Append(value: "<li style='margin-top:12px'>");

            _data.AppendFormat(CultureInfo.InvariantCulture,
                format: "<a dir='{3}' href='{0}questions/details/{1}'><b>{2}</b></a><br/>", url, question.Id,
                question.Title, question.Title.GetDir());

            var name = question.User is null
                ? SharedConstants.GuestUserName
                : question.User.FriendlyName ?? SharedConstants.GuestUserName;

            _data.Append(name);
            _data.AppendFormat(CultureInfo.InvariantCulture, format: "{0}<br/>", Hr);
            _data.Append(WrapContentWithCorrectDirection(question.Description.GetBriefDescription(charLength: 200)));
            _data.Append(value: "<br/><br/>");
            _data.Append(value: "</li>");
        }

        _data.Append(value: "</ul>");
    }
}
