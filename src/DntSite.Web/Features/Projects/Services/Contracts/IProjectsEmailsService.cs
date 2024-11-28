using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IProjectsEmailsService : IScopedService
{
    public Task NewProjectEmailToUserAsync(int id, ProjectModel data, int userId);

    public Task NewProjectEmailToAdminsAsync(int id, ProjectModel data);

    public Task NewIssueSendEmailToAdminsAsync(ProjectIssue data);

    public Task NewIssueSendEmailToProjectWritersAsync(ProjectIssue data);

    public Task SendApplyIssueStatusEmailToAdminsAndPersonAsync(int? toUserId,
        string toUserIdEmail,
        int projectId,
        int issueId,
        string issueTitle,
        string projectName,
        string statusName);

    public Task ProjectIssueCommentSendEmailToAdminsAsync(ProjectIssueComment data);

    public Task ProjectIssueCommentSendEmailToWritersAsync(ProjectIssueComment issueComment);

    public Task ProjectIssueCommentSendEmailToPersonAsync(ProjectIssueComment issueComment);

    public Task SendNewFaqEmailAsync(int projectId, int faqId, string title, string body);
}
