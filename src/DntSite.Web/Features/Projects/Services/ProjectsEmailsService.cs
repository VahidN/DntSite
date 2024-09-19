using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Projects.EmailLayouts;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.Projects.Services.Contracts;

namespace DntSite.Web.Features.Projects.Services;

public class ProjectsEmailsService(ICommonService commonService, IEmailsFactoryService emailsFactoryService)
    : IProjectsEmailsService
{
    public async Task NewIssueSendEmailToAdminsAsync(ProjectIssue data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var project = await commonService.FindProjectAsync(data.ProjectId);

        if (project is null)
        {
            return;
        }

        await emailsFactoryService.SendEmailToAllAdminsAsync<NewIssueSendEmailToAdmins, NewIssueSendEmailToAdminsModel>(
            string.Create(CultureInfo.InvariantCulture, $"ProjectId/{data.ProjectId}/PmId/{data.Id}"), inReplyTo: "",
            string.Create(CultureInfo.InvariantCulture, $"ProjectId/{data.ProjectId}/PmId/{data.Id}"),
            new NewIssueSendEmailToAdminsModel
            {
                Title = project.Title,
                Username = data.GuestUser.UserName,
                Body = data.Description,
                PmId = data.Id.ToString(CultureInfo.InvariantCulture),
                Stat = "عمومی",
                ProjectId = data.ProjectId.ToString(CultureInfo.InvariantCulture)
            }, $"بازخورد جدید : {data.Title}");
    }

    public async Task NewIssueSendEmailToProjectWritersAsync(ProjectIssue data)
    {
        ArgumentNullException.ThrowIfNull(data);

        if (data.IsDeleted)
        {
            return;
        }

        var project = await commonService.FindProjectAsync(data.ProjectId);

        if (project is null)
        {
            return;
        }

        var authorId = project.UserId;

        if (data.UserId.HasValue && authorId is not null && authorId.Value == data.UserId.Value)
        {
            return; //don't send emails to me again.
        }

        if (authorId is not null)
        {
            await emailsFactoryService.SendEmailToIdAsync<NewIssueSendEmailToWriters, NewIssueSendEmailToWritersModel>(
                string.Create(CultureInfo.InvariantCulture, $"ProjectId/{data.ProjectId}/PmId/{data.Id}"),
                inReplyTo: "",
                string.Create(CultureInfo.InvariantCulture, $"ProjectId/{data.ProjectId}/PmId/{data.Id}"),
                new NewIssueSendEmailToWritersModel
                {
                    Title = project.Title,
                    Username = data.GuestUser.UserName,
                    Body = data.Description,
                    PmId = data.Id.ToString(CultureInfo.InvariantCulture),
                    ProjectId = data.ProjectId.ToString(CultureInfo.InvariantCulture)
                }, authorId.Value, $"بازخورد جدید : {data.Title}");
        }
    }

    public Task NewProjectEmailToAdminsAsync(int id, ProjectModel data)
    {
        ArgumentNullException.ThrowIfNull(data);

        return emailsFactoryService.SendEmailToAllAdminsAsync<NewProjectEmail, NewProjectEmailModel>(
            messageId: "NewProject", inReplyTo: "", references: "NewProject", new NewProjectEmailModel
            {
                Title = data.Title,
                DescriptionText = data.DescriptionText,
                DevelopersDescription = data.DevelopersDescriptionText ?? "",
                License = data.LicenseText,
                RelatedArticles = data.RelatedArticlesText ?? "",
                RequiredDependencies = data.RequiredDependenciesText ?? "",
                SourcecodeRepositoryUrl = data.SourcecodeRepositoryUrl,
                Id = id.ToString(CultureInfo.InvariantCulture)
            }, $"پروژه جدید : {data.Title}");
    }

    public Task NewProjectEmailToUserAsync(int id, ProjectModel data, int userId)
    {
        ArgumentNullException.ThrowIfNull(data);

        return emailsFactoryService.SendEmailToIdAsync<NewProjectHelpEmail, NewProjectHelpEmailModel>(
            messageId: "NewProject", inReplyTo: "", references: "NewProject", new NewProjectHelpEmailModel
            {
                Title = data.Title,
                Id = id.ToString(CultureInfo.InvariantCulture)
            }, userId, $"پروژه جدید : {data.Title}");
    }

    public Task ProjectIssueCommentSendEmailToAdminsAsync(ProjectIssueComment data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var issue = data.Parent;

        return emailsFactoryService.SendEmailToAllAdminsAsync<IssueReplyToAdminsEmail, IssueReplyToAdminsEmailModel>(
            string.Create(CultureInfo.InvariantCulture, $"ProjectId/{issue.ProjectId}/PmId/{data.ParentId}"),
            inReplyTo: "",
            string.Create(CultureInfo.InvariantCulture, $"ProjectId/{issue.ProjectId}/PmId/{data.ParentId}"),
            new IssueReplyToAdminsEmailModel
            {
                Title = issue.Title,
                Username = data.GuestUser.UserName,
                Body = data.Body,
                PmId = data.ParentId.ToString(CultureInfo.InvariantCulture),
                Stat = "عمومی",
                CommentId = data.Id.ToString(CultureInfo.InvariantCulture),
                ProjectId = issue.ProjectId.ToString(CultureInfo.InvariantCulture)
            }, $"پاسخ به : {issue.Title}");
    }

    public async Task ProjectIssueCommentSendEmailToPersonAsync(ProjectIssueComment issueComment)
    {
        ArgumentNullException.ThrowIfNull(issueComment);

        var replyId = issueComment.ReplyId;

        if (replyId is null)
        {
            return;
        }

        if (issueComment.IsDeleted)
        {
            return;
        }

        var issue = issueComment.Parent;

        if (issueComment.UserId.HasValue && IsIssueCommentatorAuthorOfIssue(issueComment, issue))
        {
            return; //don't send emails to me again.
        }

        var replyToComment = await commonService.FindIssueCommentAsync(replyId.Value);

        if (replyToComment is null)
        {
            return;
        }

        if (IsIssueAnonymousUser(replyToComment))
        {
            if (!replyToComment.GuestUser.Email.IsValidEmail())
            {
                return;
            }

            await emailsFactoryService.SendEmailAsync<IssueReplyToPersonEmail, IssueReplyToPersonEmailModel>(
                string.Create(CultureInfo.InvariantCulture,
                    $"ProjectId/{issue.ProjectId}/PmId/{issueComment.ParentId}"), inReplyTo: "",
                string.Create(CultureInfo.InvariantCulture,
                    $"ProjectId/{issue.ProjectId}/PmId/{issueComment.ParentId}"), new IssueReplyToPersonEmailModel
                {
                    Title = issue.Title,
                    ReplyToComment = replyToComment.Body,
                    Username = issueComment.GuestUser.UserName,
                    Body = issueComment.Body,
                    PmId = issueComment.ParentId.ToString(CultureInfo.InvariantCulture),
                    CommentId = issueComment.Id.ToString(CultureInfo.InvariantCulture),
                    ProjectId = issue.ProjectId.ToString(CultureInfo.InvariantCulture)
                }, replyToComment.GuestUser.Email, $"پاسخ به : {issue.Title}", addIp: false);

            return;
        }

        if (IsIssueCommentatorMe(issueComment, replyToComment))
        {
            return;
        }

        if (replyToComment.UserId is not null)
        {
            await emailsFactoryService.SendEmailToIdAsync<IssueReplyToPersonEmail, IssueReplyToPersonEmailModel>(
                string.Create(CultureInfo.InvariantCulture,
                    $"ProjectId/{issue.ProjectId}/PmId/{issueComment.ParentId}"), inReplyTo: "",
                string.Create(CultureInfo.InvariantCulture,
                    $"ProjectId/{issue.ProjectId}/PmId/{issueComment.ParentId}"), new IssueReplyToPersonEmailModel
                {
                    Title = issue.Title,
                    ReplyToComment = replyToComment.Body,
                    Username = issueComment.GuestUser.UserName,
                    Body = issueComment.Body,
                    PmId = issueComment.ParentId.ToString(CultureInfo.InvariantCulture),
                    CommentId = issueComment.Id.ToString(CultureInfo.InvariantCulture),
                    ProjectId = issue.ProjectId.ToString(CultureInfo.InvariantCulture)
                }, replyToComment.UserId.Value, $"پاسخ به : {issue.Title}");
        }
    }

    public async Task ProjectIssueCommentSendEmailToWritersAsync(ProjectIssueComment issueComment)
    {
        ArgumentNullException.ThrowIfNull(issueComment);

        if (issueComment.IsDeleted)
        {
            return;
        }

        var issue = issueComment.Parent;

        if (issueComment.UserId.HasValue && IsIssueCommentatorAuthorOfIssue(issueComment, issue))
        {
            return; //don't send emails to me again.
        }

        var items = new IssueReplyToWritersEmailModel
        {
            Title = issue.Title,
            Username = issueComment.GuestUser.UserName,
            Body = issueComment.Body,
            PmId = issueComment.ParentId.ToString(CultureInfo.InvariantCulture),
            CommentId = issueComment.Id.ToString(CultureInfo.InvariantCulture),
            ProjectId = issue.ProjectId.ToString(CultureInfo.InvariantCulture)
        };

        if (issue.UserId is not null)
        {
            await emailsFactoryService.SendEmailToIdAsync<IssueReplyToWritersEmail, IssueReplyToWritersEmailModel>(
                string.Create(CultureInfo.InvariantCulture,
                    $"ProjectId/{issue.ProjectId}/PmId/{issueComment.ParentId}"), inReplyTo: "",
                string.Create(CultureInfo.InvariantCulture,
                    $"ProjectId/{issue.ProjectId}/PmId/{issueComment.ParentId}"), items, issue.UserId.Value,
                $"پاسخ به : {issue.Title}");
        }
        else
        {
            await emailsFactoryService.SendEmailAsync<IssueReplyToWritersEmail, IssueReplyToWritersEmailModel>(
                string.Create(CultureInfo.InvariantCulture,
                    $"ProjectId/{issue.ProjectId}/PmId/{issueComment.ParentId}"), inReplyTo: "",
                string.Create(CultureInfo.InvariantCulture,
                    $"ProjectId/{issue.ProjectId}/PmId/{issueComment.ParentId}"), items, issue.GuestUser.Email,
                $"پاسخ به : {issue.Title}", addIp: false);
        }
    }

    public async Task SendApplyIssueStatusEmailToAdminsAndPersonAsync(int? toUserId,
        string toUserIdEmail,
        int projectId,
        int issueId,
        string issueTitle,
        string projectName,
        string statusName)
    {
        var items = new ApplyIssueStatusEmailModel
        {
            ProjectId = projectId.ToString(CultureInfo.InvariantCulture),
            IssueId = issueId.ToString(CultureInfo.InvariantCulture),
            IssueTitle = issueTitle,
            ProjectName = projectName,
            StatusName = statusName
        };

        var emailTitle = $"تغییر وضعیت بازخورد : {issueTitle}";

        if (toUserId is not null)
        {
            await emailsFactoryService.SendEmailToIdAsync<ApplyIssueStatusEmail, ApplyIssueStatusEmailModel>(
                string.Create(CultureInfo.InvariantCulture, $"ProjectId/{projectId}/PmId/{issueId}"), inReplyTo: "",
                string.Create(CultureInfo.InvariantCulture, $"ProjectId/{projectId}/PmId/{issueId}"), items,
                toUserId.Value, emailTitle);
        }
        else
        {
            await emailsFactoryService.SendEmailAsync<ApplyIssueStatusEmail, ApplyIssueStatusEmailModel>(
                string.Create(CultureInfo.InvariantCulture, $"ProjectId/{projectId}/PmId/{issueId}"), inReplyTo: "",
                string.Create(CultureInfo.InvariantCulture, $"ProjectId/{projectId}/PmId/{issueId}"), items,
                toUserIdEmail, emailTitle, addIp: false);
        }

        await emailsFactoryService.SendEmailToAllAdminsAsync<ApplyIssueStatusEmail, ApplyIssueStatusEmailModel>(
            string.Create(CultureInfo.InvariantCulture, $"ProjectId/{projectId}/PmId/{issueId}"), inReplyTo: "",
            string.Create(CultureInfo.InvariantCulture, $"ProjectId/{projectId}/PmId/{issueId}"), items, emailTitle);
    }

    public Task SendNewFaqEmailAsync(int projectId, int faqId, string title, string body)
    {
        var items = new ProjectFaqEmailModel
        {
            ProjectId = projectId.ToString(CultureInfo.InvariantCulture),
            FaqId = faqId.ToString(CultureInfo.InvariantCulture),
            Title = title,
            Body = body
        };

        return emailsFactoryService.SendEmailToAllAdminsAsync<ProjectFaqEmail, ProjectFaqEmailModel>(
            messageId: "NewFaq", inReplyTo: "", references: "NewFaq", items, $"راهنمای جدید: {title}");
    }

    private static bool IsIssueAnonymousUser(ProjectIssueComment replyToComment) => replyToComment.UserId is null;

    private static bool IsIssueCommentatorAuthorOfIssue(ProjectIssueComment comment, ProjectIssue post)
        => comment.UserId is not null && comment.UserId.Value == post.UserId;

    private static bool IsIssueCommentatorMe(ProjectIssueComment comment, ProjectIssueComment replyToComment)
        => replyToComment.UserId is not null && comment.UserId.HasValue &&
           replyToComment.UserId.Value == comment.UserId.Value;
}
