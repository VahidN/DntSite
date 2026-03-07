using System.Text;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;
using DntSite.Web.Features.Courses.RoutingConstants;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Courses.ModelsMappings;

public static class CourseMappersExtensions
{
    public const string CourseTags = $"{nameof(Course)}_Tags";

    private static readonly CompositeFormat ParsedPostUrlTemplate =
        CompositeFormat.Parse(CoursesRoutingConstants.PostUrlTemplate);

    public static WhatsNewItemModel MapToWhatsNewItemModel(this CourseTopicComment item,
        string siteRootUri,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = showBriefDescription ? item.Body.GetBriefDescription(charLength: 450) : item.Body,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.CourseTopicsReplies.Value}: {item.Parent.Title}",
            OriginalTitle = item.Parent.Title,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture,
                    $"{CoursesRoutingConstants.CoursesTopicBase}/{item.Parent.CourseId}/{item.Parent.DisplayId:D}#comment-{item.Id}"),
                escapeRelativeUrl: false),
            Categories = [],
            ItemType = WhatsNewItemType.CourseTopicsReplies,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToWhatsNewItemModel(this CourseTopic item,
        string siteRootUri,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = showBriefDescription ? item.Body.GetBriefDescription(charLength: 450) : item.Body,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.AllCoursesTopics.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture,
                    $"{CoursesRoutingConstants.CoursesTopicBase}/{item.CourseId}/{item.DisplayId:D}"),
                escapeRelativeUrl: false),
            Categories = item.Course.Tags.Select(x => x.Name),
            ItemType = WhatsNewItemType.AllCoursesTopics,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToWhatsNewItemModel(this Course item,
        string siteRootUri,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = showBriefDescription ? item.Description.GetBriefDescription(charLength: 450) : item.Description,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.AllCourses.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture, ParsedPostUrlTemplate, item.Id),
                escapeRelativeUrl: false),
            Categories = item.Tags.Select(x => x.Name),
            ItemType = WhatsNewItemType.AllCourses,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static Course MapCourseModelToCourse(this CourseModel source,
        IAppAntiXssService antiXssService,
        Course? destination = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(antiXssService);

        var course = new Course
        {
            Title = source.Title,
            Description = antiXssService.GetSanitizedHtml(source.Description),
            Requirements = antiXssService.GetSanitizedHtml(source.Requirements),
            Perm = source.Perm,
            IsFree = source.Perm != CourseType.IsNotFree
        };

        switch (source.Perm)
        {
            // تنظیم بر اساس نوع دسترسی
            case CourseType.FreeForAll or CourseType.IsNotFree:
                course.NumberOfPostsRequired = 0;
                course.NumberOfMonthsRequired = 0;
                course.NumberOfTotalRatingsRequired = 0;
                course.NumberOfMonthsTotalRatingsRequired = 0;

                break;
            case CourseType.FreeForActiveUsers:
                course.NumberOfPostsRequired = 0;
                course.NumberOfMonthsRequired = 0;

                break;
            case CourseType.FreeForWriters:
                course.NumberOfTotalRatingsRequired = 0;
                course.NumberOfMonthsTotalRatingsRequired = 0;

                break;
        }

        if (destination is not null)
        {
            destination.Title = course.Title;
            destination.Description = course.Description;
            destination.Requirements = course.Requirements;
            destination.Perm = course.Perm;
            destination.IsFree = course.IsFree;
            destination.NumberOfPostsRequired = course.NumberOfPostsRequired;
            destination.NumberOfMonthsRequired = course.NumberOfMonthsRequired;
            destination.NumberOfTotalRatingsRequired = course.NumberOfTotalRatingsRequired;
            destination.NumberOfMonthsTotalRatingsRequired = course.NumberOfMonthsTotalRatingsRequired;
        }

        return destination ?? course;
    }

    public static CourseModel MapCourseToCourseModel(this Course source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new CourseModel
        {
            Title = source.Title,
            Description = source.Description,
            Requirements = source.Requirements ?? "",
            Perm = source.Perm,
            Tags = source.Tags?.Select(tag => tag.Name).ToList() ?? []
        };
    }

    public static CourseTopic MapCourseTopicItemModelToCourseTopic(this CourseTopicItemModel source,
        IAppAntiXssService antiXssService,
        CourseTopic? destination = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(antiXssService);

        var body = antiXssService.GetSanitizedHtml(source.Body);

        var courseTopic = new CourseTopic
        {
            Title = source.Title,
            Body = body,
            ReadingTimeMinutes = body.MinReadTime(),
            DisplayId = Guid.NewGuid()
        };

        if (destination is not null)
        {
            destination.Title = courseTopic.Title;
            destination.Body = courseTopic.Body;
            destination.ReadingTimeMinutes = courseTopic.ReadingTimeMinutes;
            destination.DisplayId = courseTopic.DisplayId;
        }

        return destination ?? courseTopic;
    }

    public static CourseTopicItemModel MapCourseTopicToCourseTopicItemModel(this CourseTopic source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new CourseTopicItemModel
        {
            Title = source.Title,
            Body = source.Body,
            IsMainTopic = source.IsMainTopic
        };
    }
}
