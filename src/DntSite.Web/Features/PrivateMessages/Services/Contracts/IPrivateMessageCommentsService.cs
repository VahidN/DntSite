using DntSite.Web.Features.PrivateMessages.Entities;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.PrivateMessages.Services.Contracts;

public interface IPrivateMessageCommentsService : IScopedService
{
    public Task<PrivateMessageComment?> GetReplyToMessageAsync(int? replyId);

    public Task<List<PrivateMessageComment>> GetRootCommentsOfPrivateMessageAsync(int pmId,
        int count = 1000,
        bool showDeletedItems = false);

    public Task DeleteCommentAsync(int? commentId);

    public Task EditReplyAsync(int? commentId, string message);

    public Task AddReplyAsync(int? replyId, int blogPostId, string message, User? fromUser);

    public ValueTask<PrivateMessageComment?> FindCommentAsync(int id);

    public PrivateMessageComment AddComment(PrivateMessageComment data);

    public Task SendReplyEmailsAsync(int blogPostId, User? fromUser, string message);
}
