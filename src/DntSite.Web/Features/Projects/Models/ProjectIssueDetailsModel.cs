using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.Models;

public class ProjectIssueDetailsModel
{
    public ProjectIssue? CurrentItem { set; get; }

    public ProjectIssue? NextItem { set; get; }

    public ProjectIssue? PreviousItem { set; get; }

    public IList<ProjectIssueComment> CommentsList { set; get; } = [];

    public IList<ProjectIssueStatus> IssueStatus { set; get; } = [];
}
