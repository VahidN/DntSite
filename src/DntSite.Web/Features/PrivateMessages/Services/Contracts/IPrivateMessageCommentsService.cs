using DntSite.Web.Features.PrivateMessages.Entities;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.PrivateMessages.Services.Contracts;

public interface IPrivateMessageCommentsService : IScopedService
{
    Task<PrivateMessageComment?> GetReplyToMessageAsync(int? replyId);

    Task<List<PrivateMessageComment>> GetRootCommentsOfPrivateMessageAsync(int pmId,
        int count = 1000,
        bool showDeletedItems = false);

    Task DeleteCommentAsync(int? commentId);

    Task EditReplyAsync(int? commentId, string message);

    Task AddReplyAsync(int? replyId, int blogPostId, string message, User? fromUser);

    ValueTask<PrivateMessageComment?> FindCommentAsync(int id);

    PrivateMessageComment AddComment(PrivateMessageComment data);

    Task SendReplyEmailsAsync(int blogPostId, User? fromUser, string message);
}
