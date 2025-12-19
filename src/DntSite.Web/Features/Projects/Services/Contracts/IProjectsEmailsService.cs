using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IProjectsEmailsService : IScopedService
{
    Task NewProjectEmailToUserAsync(int id, ProjectModel data, int userId);

    Task NewProjectEmailToAdminsAsync(int id, ProjectModel data);

    Task NewIssueSendEmailToAdminsAsync(ProjectIssue data);

    Task NewIssueSendEmailToProjectWritersAsync(ProjectIssue data);

    Task SendApplyIssueStatusEmailToAdminsAndPersonAsync(int? toUserId,
        string toUserIdEmail,
        int projectId,
        int issueId,
        string issueTitle,
        string projectName,
        string statusName);

    Task ProjectIssueCommentSendEmailToAdminsAsync(ProjectIssueComment data);

    Task ProjectIssueCommentSendEmailToWritersAsync(ProjectIssueComment issueComment);

    Task ProjectIssueCommentSendEmailToPersonAsync(ProjectIssueComment issueComment);

    Task SendNewFaqEmailAsync(int projectId, int faqId, string title, string body);
}
