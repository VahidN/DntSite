using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Persistence.Utils;
using DntSite.Web.Features.PrivateMessages.Entities;
using DntSite.Web.Features.PrivateMessages.Models;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.PrivateMessages.Services;

public class PrivateMessageCommentsService(
    IUnitOfWork uow,
    IAntiXssService antiXssService,
    IPrivateMessagesEmailsService privateMessagesEmailsService,
    IPrivateMessagesService privateMessagesService) : IPrivateMessageCommentsService
{
    private readonly DbSet<PrivateMessageComment> _privateMessageComments = uow.DbSet<PrivateMessageComment>();

    public async Task<List<PrivateMessageComment>> GetRootCommentsOfPrivateMessageAsync(int pmId,
        int count = 1000,
        bool showDeletedItems = false)
    {
        var privateMessageComments = await _privateMessageComments.AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.ParentId == pmId && x.IsDeleted == showDeletedItems)
            .OrderBy(x => x.Id)
            .Take(count)
            .ToListAsync();

        return privateMessageComments.ToSelfReferencingTree();
    }

    public ValueTask<PrivateMessageComment?> FindCommentAsync(int id) => _privateMessageComments.FindAsync(id);

    public async Task DeleteCommentAsync(int? commentId)
    {
        if (commentId is null)
        {
            return;
        }

        var comment = await FindCommentAsync(commentId.Value);

        if (comment is null)
        {
            return;
        }

        comment.IsDeleted = true;
        await uow.SaveChangesAsync();
    }

    public async Task EditReplyAsync(int? commentId, string message)
    {
        if (commentId is null)
        {
            return;
        }

        var comment = await FindCommentAsync(commentId.Value);

        if (comment is null)
        {
            return;
        }

        comment.Body = antiXssService.GetSanitizedHtml(message);
        await uow.SaveChangesAsync();
    }

    public async Task AddReplyAsync(int? replyId, int blogPostId, string message, User? fromUser)
    {
        if (string.IsNullOrWhiteSpace(message) || fromUser is null)
        {
            return;
        }

        var comment = new PrivateMessageComment
        {
            ParentId = blogPostId,
            ReplyId = replyId,
            Body = antiXssService.GetSanitizedHtml(message),
            UserId = fromUser.Id
        };

        AddComment(comment);
        await uow.SaveChangesAsync();
    }

    public Task<PrivateMessageComment?> GetReplyToMessageAsync(int? replyId)
    {
        if (!replyId.HasValue)
        {
            return Task.FromResult<PrivateMessageComment?>(result: null);
        }

        return _privateMessageComments.Include(x => x.User)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.Id == replyId.Value);
    }

    public PrivateMessageComment AddComment(PrivateMessageComment data) => _privateMessageComments.Add(data).Entity;

    public async Task SendReplyEmailsAsync(int blogPostId, User? fromUser, string message)
    {
        var mainMessage = await privateMessagesService.GetFirstPrivateMessageAsync(blogPostId);

        if (mainMessage is null || fromUser is null)
        {
            return;
        }

        var mainMessageUser = mainMessage.User!;
        var mainMessageToUser = mainMessage.ToUser;
        var toUser = mainMessageUser.Id == fromUser.Id ? mainMessageToUser : mainMessageUser;

        await privateMessagesEmailsService.SendEmailContactUsAsync(new ContactUsModel
        {
            DescriptionText = antiXssService.GetSanitizedHtml(message),
            Title = mainMessage.Title
        }, toUser, fromUser, blogPostId);
    }
}
