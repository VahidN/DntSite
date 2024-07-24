using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.PrivateMessages.Entities;
using DntSite.Web.Features.PrivateMessages.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.PrivateMessages.Services.Contracts;

public interface IPrivateMessagesService : IScopedService
{
    Task<PrivateMessage> AddPrivateMessageAsync(PrivateMessage pm);

    Task<OperationResult> AddPrivateMessageAsync(User? fromUser, int? toUserId, ContactUsModel? model);

    Task TryMarkMainMessageAsReadAsync(int messageId, int? currentUserId);

    Task TryMarkMainMessageAsUnReadAsync(int messageId, int? currentUserId);

    Task EditFirstPrivateMessageAsync(int id, int? userId, ContactUsModel model);

    Task RemovePrivateMessageAsync(int id);

    ValueTask<PrivateMessage?> FindPrivateMessageAsync(int id);

    Task<PrivateMessage?> GetFirstAllowedPrivateMessageAsync(int? id, int? userId);

    Task<PrivateMessage?> GetFirstPrivateMessageAsync(int messageId, bool showDeletedItems = false);

    Task<PagedResultModel<PrivateMessage>> GetUserPrivateMessagesAsNoTrackingAsync(int userId,
        int pageNumber,
        int recordsPerPage = 20,
        bool showDeletedItems = false);

    Task<List<PrivateMessage>> GetAllPrivateMessagesOfThisIdAsync(int firstMessageId,
        int count = 1000,
        bool showDeletedItems = false);

    Task<List<PrivateMessage>> GetRootPrivateMessagesAsync(int firstMessageId,
        int count = 1000,
        bool showDeletedItems = false);

    Task<PagedResultModel<PrivateMessage>> GetUserSentPrivateMessagesAsNoTrackingAsync(int userId,
        int pageNumber,
        int recordsPerPage = 20,
        bool showDeletedItems = false);

    Task<PagedResultModel<PrivateMessage>> GetAllPrivateMessagesAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 20);

    Task<List<PrivateMessage>> GetAllPrivateMessagesOfThisIdAsNoTrackingAsync(int firstMessageId,
        int count = 1000,
        bool showDeletedItems = false);

    Task DeleteAllAsync();

    Task<int> GetUserUnReadPrivateMessagesCountAsync(int? userId);

    Task<PagedResultModel<PrivateMessage>> GetAllUserPrivateMessagesAsNoTrackingAsync(int userId,
        int pageNumber,
        int recordsPerPage = 20,
        bool showDeletedItems = false);
}
