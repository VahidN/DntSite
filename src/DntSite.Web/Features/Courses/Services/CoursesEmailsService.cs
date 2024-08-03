using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Courses.EmailLayouts;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;
using DntSite.Web.Features.Courses.Services.Contracts;

namespace DntSite.Web.Features.Courses.Services;

public class CoursesEmailsService(ICommonService commonService, IEmailsFactoryService emailsFactoryService)
    : ICoursesEmailsService
{
    public Task AccessAddedEmailToAdminAsync(Course data, string userName, string operation)
    {
        ArgumentNullException.ThrowIfNull(data);

        return emailsFactoryService.SendEmailToAllAdminsAsync<AddAccessEmailToAdmin, AddAccessEmailToAdminModel>(
            messageId: "AccessAdded", inReplyTo: "", references: "AccessAdded", new AddAccessEmailToAdminModel
            {
                Title = data.Title,
                UserName = userName,
                Operation = operation
            }, $"تغییر دسترسی به جدید : {data.Title}");
    }

    public async Task AccessAddedEmailToUserAsync(Course data, string userName)
    {
        ArgumentNullException.ThrowIfNull(data);

        var user = await commonService.FindUserAsync(userName);

        if (user is null)
        {
            return;
        }

        await emailsFactoryService.SendEmailAsync<AddAccessEmailToUser, AddAccessEmailToUserModel>(
            messageId: "AccessAdded", inReplyTo: "", references: "AccessAdded", new AddAccessEmailToUserModel
            {
                Title = data.Title,
                Id = data.Id.ToString(CultureInfo.InvariantCulture)
            }, user.EMail, $"دسترسی به دوره جدید : {data.Title}", addIp: false);
    }

    public Task CourseQuestionCommentSendEmailToAdminsAsync(CourseQuestionComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        var data = comment.Parent;

        return emailsFactoryService
            .SendEmailToAllAdminsAsync<CourseQuestionReplyToAdminsEmail, CourseQuestionReplyToAdminsEmailModel>(
                Invariant($"CourseId/{data.CourseId}/courseQuestionPmQId/{comment.ParentId}"), inReplyTo: "",
                Invariant($"CourseId/{data.CourseId}/courseQuestion/{data.Id}"),
                new CourseQuestionReplyToAdminsEmailModel
                {
                    Title = data.Title,
                    Username = comment.GuestUser.UserName,
                    Body = comment.Body,
                    PmId = comment.ParentId.ToString(CultureInfo.InvariantCulture),
                    Stat = "عمومی",
                    CommentId = comment.Id.ToString(CultureInfo.InvariantCulture),
                    CourseId = data.CourseId.ToString(CultureInfo.InvariantCulture)
                }, $"پاسخ به : {data.Title}");
    }

    public async Task CourseQuestionCommentSendEmailToPersonAsync(CourseQuestionComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        var replyId = comment.ReplyId;

        if (replyId is null)
        {
            return;
        }

        if (comment.IsDeleted)
        {
            return;
        }

        var question = comment.Parent;

        if (comment.UserId.HasValue && IsCourseQuestionCommentatorAuthorOfIssue(comment, question))
        {
            return; //don't send emails to me again.
        }

        var replyToComment = await commonService.FindCourseQuestionCommentAsync(replyId.Value);

        if (replyToComment is null)
        {
            return;
        }

        if (IsCourseQuestionCommentAnonymousUser(replyToComment))
        {
            if (!replyToComment.GuestUser.Email.IsValidEmail())
            {
                return;
            }

            await emailsFactoryService
                .SendEmailAsync<CourseQuestionReplyToPersonEmail, CourseQuestionReplyToPersonEmailModel>(
                    Invariant($"CourseId/{question.CourseId}/courseQuestionPmQId/{comment.ParentId}"), inReplyTo: "",
                    Invariant($"CourseId/{question.CourseId}/courseQuestion/{question.Id}"),
                    new CourseQuestionReplyToPersonEmailModel
                    {
                        Title = question.Title,
                        ReplyToComment = replyToComment.Body,
                        Username = comment.GuestUser.UserName,
                        Body = comment.Body,
                        PmId = comment.ParentId.ToString(CultureInfo.InvariantCulture),
                        CommentId = comment.Id.ToString(CultureInfo.InvariantCulture),
                        CourseId = question.CourseId.ToString(CultureInfo.InvariantCulture)
                    }, replyToComment.GuestUser.Email, $"پاسخ به : {question.Title}", addIp: false);

            return;
        }

        if (IsCourseQuestionCommentatorMe(comment, replyToComment))
        {
            return;
        }

        if (replyToComment.UserId is not null)
        {
            await emailsFactoryService
                .SendEmailToIdAsync<CourseQuestionReplyToPersonEmail, CourseQuestionReplyToPersonEmailModel>(
                    Invariant($"CourseId/{question.CourseId}/courseQuestionPmQId/{comment.ParentId}"), inReplyTo: "",
                    Invariant($"CourseId/{question.CourseId}/courseQuestion/{question.Id}"),
                    new CourseQuestionReplyToPersonEmailModel
                    {
                        Title = question.Title,
                        ReplyToComment = replyToComment.Body,
                        Username = comment.GuestUser.UserName,
                        Body = comment.Body,
                        PmId = comment.ParentId.ToString(CultureInfo.InvariantCulture),
                        CommentId = comment.Id.ToString(CultureInfo.InvariantCulture),
                        CourseId = question.CourseId.ToString(CultureInfo.InvariantCulture)
                    }, replyToComment.UserId.Value, $"پاسخ به : {question.Title}");
        }
    }

    public async Task CourseQuestionCommentSendEmailToWritersAsync(CourseQuestionComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        if (comment.IsDeleted)
        {
            return;
        }

        var question = comment.Parent;

        if (comment.UserId.HasValue && IsCourseQuestionCommentatorAuthorOfIssue(comment, question))
        {
            return; //don't send emails to me again.
        }

        if (question.UserId is not null)
        {
            await emailsFactoryService
                .SendEmailToIdAsync<CourseQuestionReplyToWritersEmail, CourseQuestionReplyToWritersEmailModel>(
                    Invariant($"CourseId/{question.CourseId}/courseQuestionPmQId/{comment.ParentId}"), inReplyTo: "",
                    Invariant($"CourseId/{question.CourseId}/courseQuestion/{question.Id}"),
                    new CourseQuestionReplyToWritersEmailModel
                    {
                        Title = question.Title,
                        Username = comment.GuestUser.UserName,
                        Body = comment.Body,
                        PmId = comment.ParentId.ToString(CultureInfo.InvariantCulture),
                        CommentId = comment.Id.ToString(CultureInfo.InvariantCulture),
                        CourseId = question.CourseId.ToString(CultureInfo.InvariantCulture)
                    }, question.UserId.Value, $"پاسخ به : {question.Title}");
        }
    }

    public async Task CourseTopicCommentSendEmailToAdminsAsync(CourseTopicComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        var data = comment.Parent;

        await emailsFactoryService.SendEmailToAllAdminsAsync<TopicReplyToAdminsEmail, TopicReplyToAdminsEmailModel>(
            Invariant($"CourseId/{data.CourseId}/CommentId/{comment.Id}"), inReplyTo: "",
            Invariant($"CourseId/{data.CourseId}/PmId/{data.Id}"), new TopicReplyToAdminsEmailModel
            {
                Title = data.Title,
                Username = comment.GuestUser.UserName,
                Body = comment.Body,
                PmId = data.DisplayId.ToString(format: "D"),
                Stat = "عمومی",
                CommentId = comment.Id.ToString(CultureInfo.InvariantCulture),
                CId = data.CourseId.ToString(CultureInfo.InvariantCulture)
            }, $"پاسخ به : {data.Title}");
    }

    public async Task CourseTopicCommentSendEmailToPersonAsync(CourseTopicComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        var replyId = comment.ReplyId;

        if (replyId is null)
        {
            return;
        }

        if (comment.IsDeleted)
        {
            return;
        }

        var topic = comment.Parent;

        if (comment.UserId.HasValue && IsTopicCommentatorAuthorOfIssue(comment, topic))
        {
            return; //don't send emails to me again.
        }

        var replyToComment = await commonService.FindTopicCommentAsync(replyId.Value);

        if (replyToComment is null)
        {
            return;
        }

        if (IsTopicCommentAnonymousUser(replyToComment))
        {
            if (!replyToComment.GuestUser.Email.IsValidEmail())
            {
                return;
            }

            await emailsFactoryService.SendEmailAsync<TopicReplyToPersonEmail, TopicReplyToPersonEmailModel>(
                Invariant($"CourseId/{topic.CourseId}/CommentId/{comment.Id}"), inReplyTo: "",
                Invariant($"CourseId/{topic.CourseId}/PmId/{topic.Id}"), new TopicReplyToPersonEmailModel
                {
                    Title = topic.Title,
                    ReplyToComment = replyToComment.Body,
                    Username = comment.GuestUser.UserName,
                    Body = comment.Body,
                    PmId = comment.Parent.DisplayId.ToString(format: "D"),
                    CommentId = comment.Id.ToString(CultureInfo.InvariantCulture),
                    CId = topic.CourseId.ToString(CultureInfo.InvariantCulture)
                }, replyToComment.GuestUser.Email, $"پاسخ به : {topic.Title}", addIp: false);

            return;
        }

        if (IsTopicCommentatorMe(comment, replyToComment))
        {
            return;
        }

        if (replyToComment.UserId is not null)
        {
            await emailsFactoryService.SendEmailToIdAsync<TopicReplyToPersonEmail, TopicReplyToPersonEmailModel>(
                Invariant($"CourseId/{topic.CourseId}/CommentId/{comment.Id}"), inReplyTo: "",
                Invariant($"CourseId/{topic.CourseId}/PmId/{topic.Id}"), new TopicReplyToPersonEmailModel
                {
                    Title = topic.Title,
                    ReplyToComment = replyToComment.Body,
                    Username = comment.GuestUser.UserName,
                    Body = comment.Body,
                    PmId = comment.Parent.DisplayId.ToString(format: "D"),
                    CommentId = comment.Id.ToString(CultureInfo.InvariantCulture),
                    CId = topic.CourseId.ToString(CultureInfo.InvariantCulture)
                }, replyToComment.UserId.Value, $"پاسخ به : {topic.Title}");
        }
    }

    public async Task CourseTopicCommentSendEmailToWritersAsync(CourseTopicComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        if (comment.IsDeleted)
        {
            return;
        }

        var topic = comment.Parent;

        if (comment.UserId.HasValue && IsTopicCommentatorAuthorOfIssue(comment, topic))
        {
            return; //don't send emails to me again.
        }

        if (topic.UserId is not null)
        {
            await emailsFactoryService.SendEmailToIdAsync<TopicReplyToWritersEmail, TopicReplyToWritersEmailModel>(
                Invariant($"CourseId/{topic.CourseId}/CommentId/{comment.Id}"), inReplyTo: "",
                Invariant($"CourseId/{topic.CourseId}/PmId/{topic.Id}"), new TopicReplyToWritersEmailModel
                {
                    Title = topic.Title,
                    Username = comment.GuestUser.UserName,
                    Body = comment.Body,
                    PmId = comment.Parent.DisplayId.ToString(format: "D"),
                    CommentId = comment.Id.ToString(CultureInfo.InvariantCulture),
                    CId = topic.CourseId.ToString(CultureInfo.InvariantCulture)
                }, topic.UserId.Value, $"پاسخ به : {topic.Title}");
        }
    }

    public Task NewCourseEmailToAdminsAsync(int id, CourseModel data)
    {
        ArgumentNullException.ThrowIfNull(data);

        return emailsFactoryService.SendEmailToAllAdminsAsync<NewCourseEmail, NewCourseEmailModel>(
            messageId: "NewCourse", inReplyTo: "", references: "NewCourse", new NewCourseEmailModel
            {
                Title = data.Title,
                Description = data.Description,
                HowToPay = data.HowToPay,
                Requirements = data.Requirements,
                TopicsList = data.TopicsList,
                Id = id.ToString(CultureInfo.InvariantCulture)
            }, $"دوره جدید : {data.Title}");
    }

    public Task NewCourseEmailToUserAsync(int id, CourseModel data, int userId)
    {
        ArgumentNullException.ThrowIfNull(data);

        return emailsFactoryService.SendEmailToIdAsync<NewCourseHelpEmail, NewCourseHelpEmailModel>(
            messageId: "NewCourse", inReplyTo: "", references: "NewCourse", new NewCourseHelpEmailModel
            {
                Title = data.Title,
                Id = id.ToString(CultureInfo.InvariantCulture)
            }, userId, $"دوره جدید : {data.Title}");
    }

    public async Task NewQuestionSendEmailToAdminsAsync(CourseQuestion courseQuestion)
    {
        ArgumentNullException.ThrowIfNull(courseQuestion);

        var course = await commonService.FindCourseAsync(courseQuestion.CourseId);

        if (course is null)
        {
            return;
        }

        await emailsFactoryService
            .SendEmailToAllAdminsAsync<NewQuestionSendEmailToAdmins, NewQuestionSendEmailToAdminsModel>(
                Invariant($"CourseId/{courseQuestion.CourseId}/courseQuestion/{courseQuestion.Id}"), inReplyTo: "",
                Invariant($"CourseId/{courseQuestion.CourseId}/courseQuestion/{courseQuestion.Id}"),
                new NewQuestionSendEmailToAdminsModel
                {
                    Title = course.Title,
                    Username = courseQuestion.GuestUser.UserName,
                    Body = courseQuestion.Description,
                    PmId = courseQuestion.Id.ToString(CultureInfo.InvariantCulture),
                    Stat = "عمومی",
                    CourseId = courseQuestion.CourseId.ToString(CultureInfo.InvariantCulture)
                }, $"بازخورد جدید : {courseQuestion.Title}");
    }

    public async Task NewQuestionSendEmailToCourseWritersAsync(CourseQuestion data)
    {
        ArgumentNullException.ThrowIfNull(data);

        if (data.IsDeleted)
        {
            return;
        }

        var course = await commonService.FindCourseAsync(data.CourseId);

        if (course is null)
        {
            return;
        }

        var authorId = course.UserId;

        if (data.UserId.HasValue && authorId is not null && authorId.Value == data.UserId.Value)
        {
            return; //don't send emails to me again.
        }

        if (authorId is not null)
        {
            await emailsFactoryService
                .SendEmailToIdAsync<NewQuestionSendEmailToWriters, NewQuestionSendEmailToWritersModel>(
                    Invariant($"CourseId/{data.CourseId}/courseQuestion/{data.Id}"), inReplyTo: "",
                    Invariant($"CourseId/{data.CourseId}/courseQuestion/{data.Id}"),
                    new NewQuestionSendEmailToWritersModel
                    {
                        Title = course.Title,
                        Username = data.GuestUser.UserName,
                        Body = data.Description,
                        PmId = data.Id.ToString(CultureInfo.InvariantCulture),
                        CourseId = data.CourseId.ToString(CultureInfo.InvariantCulture)
                    }, authorId.Value, $"بازخورد جدید : {data.Title}");
        }
    }

    public Task WriteCourseTopicSendEmailAsync(CourseTopic data)
    {
        ArgumentNullException.ThrowIfNull(data);

        return emailsFactoryService.SendEmailToAllAdminsAsync<NewCourseTopicEmail, NewCourseTopicEmailModel>(
            messageId: "CourseTopic", inReplyTo: "", references: "CourseTopic", new NewCourseTopicEmailModel
            {
                Title = data.Title,
                Body = data.Body,
                PmId = data.DisplayId.ToString(format: "D"),
                CId = data.Id.ToString(CultureInfo.InvariantCulture)
            }, $"مطلب دوره جدید: {data.Title}");
    }

    private static bool IsCourseQuestionCommentAnonymousUser(CourseQuestionComment replyToComment)
        => replyToComment.UserId is null;

    private static bool IsCourseQuestionCommentatorAuthorOfIssue(CourseQuestionComment comment, CourseQuestion question)
        => question.UserId is not null && comment.UserId is not null && comment.UserId.Value == question.UserId.Value;

    private static bool IsCourseQuestionCommentatorMe(CourseQuestionComment comment,
        CourseQuestionComment replyToComment)
        => replyToComment.UserId is not null && comment.UserId.HasValue &&
           replyToComment.UserId.Value == comment.UserId.Value;

    private static bool IsTopicCommentAnonymousUser(CourseTopicComment replyToComment) => replyToComment.UserId is null;

    private static bool IsTopicCommentatorAuthorOfIssue(CourseTopicComment comment, CourseTopic topic)
        => comment.UserId is not null && comment.UserId.Value == topic.UserId;

    private static bool IsTopicCommentatorMe(CourseTopicComment comment, CourseTopicComment replyToComment)
        => replyToComment.UserId is not null && comment.UserId.HasValue &&
           replyToComment.UserId.Value == comment.UserId.Value;
}
