using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.PrivateMessages.Entities;
using DntSite.Web.Features.PrivateMessages.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.PrivateMessages.Services.Contracts;

public interface IPrivateMessagesService : IScopedService
{
    public Task<PrivateMessage> AddPrivateMessageAsync(PrivateMessage pm);

    public Task<OperationResult> AddPrivateMessageAsync(User? fromUser, int? toUserId, ContactUsModel? model);

    public Task TryMarkMainMessageAsReadAsync(int messageId, int? currentUserId);

    public Task TryMarkMainMessageAsUnReadAsync(int messageId, int? currentUserId);

    public Task EditFirstPrivateMessageAsync(int id, int? userId, ContactUsModel model);

    public Task RemovePrivateMessageAsync(int id);

    public ValueTask<PrivateMessage?> FindPrivateMessageAsync(int id);

    public Task<PrivateMessage?> GetFirstAllowedPrivateMessageAsync(int? id, int? userId);

    public Task<PrivateMessage?> GetFirstPrivateMessageAsync(int messageId, bool showDeletedItems = false);

    public Task<PagedResultModel<PrivateMessage>> GetUserPrivateMessagesAsNoTrackingAsync(int userId,
        int pageNumber,
        int recordsPerPage = 20,
        bool showDeletedItems = false);

    public Task<List<PrivateMessage>> GetAllPrivateMessagesOfThisIdAsync(int firstMessageId,
        int count = 1000,
        bool showDeletedItems = false);

    public Task<List<PrivateMessage>> GetRootPrivateMessagesAsync(int firstMessageId,
        int count = 1000,
        bool showDeletedItems = false);

    public Task<PagedResultModel<PrivateMessage>> GetUserSentPrivateMessagesAsNoTrackingAsync(int userId,
        int pageNumber,
        int recordsPerPage = 20,
        bool showDeletedItems = false);

    public Task<PagedResultModel<PrivateMessage>> GetAllPrivateMessagesAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 20);

    public Task<List<PrivateMessage>> GetAllPrivateMessagesOfThisIdAsNoTrackingAsync(int firstMessageId,
        int count = 1000,
        bool showDeletedItems = false);

    public Task DeleteAllAsync();

    public Task<int> GetUserUnReadPrivateMessagesCountAsync(int? userId);

    public Task<PagedResultModel<PrivateMessage>> GetAllUserPrivateMessagesAsNoTrackingAsync(int userId,
        int pageNumber,
        int recordsPerPage = 20,
        bool showDeletedItems = false);
}
