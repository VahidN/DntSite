using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.RoutingConstants;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.PrivateMessages.Entities;
using DntSite.Web.Features.PrivateMessages.Models;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.PrivateMessages.Services;

public class PrivateMessagesService(
    IUnitOfWork uow,
    IAppAntiXssService antiXssService,
    IUsersInfoService usersService,
    IAppFoldersService appFoldersService,
    IPrivateMessagesEmailsService privateMessagesEmailsService) : IPrivateMessagesService
{
    private readonly DbSet<PrivateMessage> _privateMessages = uow.DbSet<PrivateMessage>();

    public Task<PagedResultModel<PrivateMessage>> GetUserPrivateMessagesAsNoTrackingAsync(int userId,
        int pageNumber,
        int recordsPerPage = 20,
        bool showDeletedItems = false)
        => _privateMessages.AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.ToUserId == userId && x.IsDeleted == showDeletedItems)
            .OrderByDescending(x => x.Id)
            .ApplyQueryablePagingAsync(pageNumber, recordsPerPage);

    public Task<PagedResultModel<PrivateMessage>> GetAllPrivateMessagesAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 20)
    {
        var queryable = _privateMessages.AsNoTracking().Include(x => x.User).OrderByDescending(x => x.Id);

        return queryable.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<PrivateMessage>> GetUserSentPrivateMessagesAsNoTrackingAsync(int userId,
        int pageNumber,
        int recordsPerPage = 20,
        bool showDeletedItems = false)
    {
        var queryable = _privateMessages.AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.UserId == userId && x.IsDeleted == showDeletedItems)
            .OrderByDescending(x => x.Id);

        return queryable.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public ValueTask<PrivateMessage?> FindPrivateMessageAsync(int id) => _privateMessages.FindAsync(id);

    public Task<List<PrivateMessage>> GetRootPrivateMessagesAsync(int firstMessageId,
        int count = 1000,
        bool showDeletedItems = false)
        => _privateMessages.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Comments)
            .Where(x => x.Id == firstMessageId && x.IsDeleted == showDeletedItems)
            .OrderBy(x => x.Id)
            .Take(count)
            .ToListAsync();

    public Task<List<PrivateMessage>> GetAllPrivateMessagesOfThisIdAsNoTrackingAsync(int firstMessageId,
        int count = 1000,
        bool showDeletedItems = false)
        => _privateMessages.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Comments)
            .Where(x => x.Id == firstMessageId && x.IsDeleted == showDeletedItems)
            .OrderBy(x => x.Id)
            .Take(count)
            .ToListAsync();

    public Task<List<PrivateMessage>> GetAllPrivateMessagesOfThisIdAsync(int firstMessageId,
        int count = 1000,
        bool showDeletedItems = false)
        => _privateMessages.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Comments)
            .Where(x => x.Id == firstMessageId && x.IsDeleted == showDeletedItems)
            .OrderBy(x => x.Id)
            .Take(count)
            .ToListAsync();

    public async Task DeleteAllAsync(CancellationToken cancellationToken)
    {
        var lastWeek = DateTime.UtcNow.AddDays(value: -7);

        var privateMessages = await _privateMessages.Where(x => x.Audit.CreatedAt <= lastWeek)
            .Include(x => x.Comments)
            .OrderByDescending(x => x.Id)
            .ToListAsync(cancellationToken);

        _privateMessages.RemoveRange(privateMessages);
        await uow.SaveChangesAsync(cancellationToken);
    }

    public Task<int> GetUserUnReadPrivateMessagesCountAsync(int? userId)
        => userId is null
            ? Task.FromResult(result: 0)
            : _privateMessages.AsNoTracking()
                .CountAsync(privateMessage => privateMessage.ToUserId == userId.Value &&
                                              !privateMessage.IsReadByReceiver && !privateMessage.IsDeleted);

    public Task<PagedResultModel<PrivateMessage>> GetAllUserPrivateMessagesAsNoTrackingAsync(int userId,
        int pageNumber,
        int recordsPerPage = 20,
        bool showDeletedItems = false)
        => _privateMessages.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.ToUser)
            .Where(x => (x.ToUserId == userId || x.UserId == userId) && x.IsDeleted == showDeletedItems)
            .OrderByDescending(x => x.Id)
            .ApplyQueryablePagingAsync(pageNumber, recordsPerPage);

    public Task<PrivateMessage?> GetFirstPrivateMessageAsync(int messageId, bool showDeletedItems = false)
        => _privateMessages.Include(x => x.User)
            .Include(x => x.ToUser)
            .Where(x => x.IsDeleted == showDeletedItems)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync(x => x.Id == messageId);

    public async Task RemovePrivateMessageAsync(int id)
    {
        var messages = await _privateMessages.Include(x => x.Comments)
            .Where(x => x.Id == id)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        _privateMessages.RemoveRange(messages);
        await uow.SaveChangesAsync();
    }

    public async Task<PrivateMessage?> GetFirstAllowedPrivateMessageAsync(int? id, int? userId)
    {
        if (!id.HasValue)
        {
            return null;
        }

        var firstPrivateMessage = await GetFirstPrivateMessageAsync(id.Value);

        return firstPrivateMessage?.CanUserReadMessage(userId) != true ? null : firstPrivateMessage;
    }

    public async Task EditFirstPrivateMessageAsync(int id, int? userId, ContactUsModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var privateMessage = await GetFirstPrivateMessageAsync(id);

        if (privateMessage?.CanUserReadMessage(userId) != true)
        {
            return;
        }

        privateMessage.Title = model.Title;
        privateMessage.Body = GetSanitizedHtml(model);

        await uow.SaveChangesAsync();
    }

    public async Task TryMarkMainMessageAsReadAsync(int messageId, int? currentUserId)
    {
        if (currentUserId is null)
        {
            return;
        }

        var message = await GetFirstPrivateMessageAsync(messageId);

        if (message is null)
        {
            return;
        }

        var isReceiver = message.ToUserId == currentUserId.Value;

        if (!isReceiver)
        {
            return;
        }

        message.IsReadByReceiver = true;
        await uow.SaveChangesAsync();
    }

    public async Task TryMarkMainMessageAsUnReadAsync(int messageId, int? currentUserId)
    {
        if (currentUserId is null)
        {
            return;
        }

        var message = await GetFirstPrivateMessageAsync(messageId);

        if (message?.UserId is null)
        {
            return;
        }

        var isSender = message.UserId.Value == currentUserId.Value;

        if (!isSender)
        {
            return;
        }

        message.IsReadByReceiver = false;
        await uow.SaveChangesAsync();
    }

    public async Task<OperationResult> AddPrivateMessageAsync(User? fromUser, int? toUserId, ContactUsModel? model)
    {
        if (model is null)
        {
            return ("اطلاعات پیام یافت نشد", OperationStat.Failed);
        }

        if (fromUser?.UserId is null)
        {
            return ("فرستنده یافت نشد", OperationStat.Failed);
        }

        var toUser = await usersService.FindUserAsync(toUserId);

        if (toUser is null)
        {
            return ("چنین نام مستعاری در بانک اطلاعاتی سیستم موجود نیست.", OperationStat.Failed);
        }

        var fromUserId = fromUser.UserId;

        if (toUser.Id == fromUserId.Value)
        {
            return ("در حال ارسال پیام خصوصی به خودتان هستید!", OperationStat.Failed);
        }

        var privateMessage = await AddPrivateMessageAsync(new PrivateMessage
        {
            Body = GetSanitizedHtml(model),
            Title = model.Title,
            EmailsSent = false,
            UserId = fromUserId.Value,
            IsDeleted = false,
            ToUserId = toUser.Id
        });

        await privateMessagesEmailsService.SendEmailContactUsAsync(model, toUser, fromUser, privateMessage.Id);

        return OperationStat.Succeeded;
    }

    public async Task<PrivateMessage> AddPrivateMessageAsync(PrivateMessage pm)
    {
        var entity = _privateMessages.Add(pm).Entity;
        await uow.SaveChangesAsync();

        return entity;
    }

    private string GetSanitizedHtml(ContactUsModel model)
        => antiXssService.GetSanitizedHtml(model.DescriptionText,
            appFoldersService.GetFolderPath(FileType.MessagesImages),
            $"{ApiUrlsRoutingConstants.File.HttpAny.MessagesImages}?name=");
}
