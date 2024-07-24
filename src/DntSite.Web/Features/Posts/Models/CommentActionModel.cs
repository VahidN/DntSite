namespace DntSite.Web.Features.Posts.Models;

public class CommentActionModel
{
    public CommentAction CommentAction { set; get; }

    public int? FormCommentId { set; get; }

    public int FormPostId { set; get; }

    public string? Comment { set; get; }
}
