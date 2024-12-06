using DNTPersianUtils.Core.Normalizer;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Services;

public class UsersInfoService(
    IUnitOfWork uow,
    ICachedAppSettingsProvider configsService,
    IPasswordHasherService passwordHasherService) : IUsersInfoService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<User, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.UserName,
        [PagerSortBy.RepliesNumbers] = x => x.UserStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.UserStat.NumberOfPosts,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<User> _users = uow.DbSet<User>();
    private readonly DbSet<UserSocialNetwork> _userUserSocialNetworks = uow.DbSet<UserSocialNetwork>();

    public async Task<bool> IsWriterAsync(int userId)
    {
        var user = await FindUserAsync(userId);

        return user is not null && user.UserStat.NumberOfPosts >= 1;
    }

    public async Task<bool> CheckEMailAsync(string eMail, int? userId)
    {
        if (string.IsNullOrWhiteSpace(eMail))
        {
            return false;
        }

        eMail = eMail.FixGmailDots();

        var bannedItems = (await configsService.GetAppSettingsAsync())?.BannedEmails;

        if (bannedItems?.Any(email => eMail.EndsWith(email, StringComparison.InvariantCultureIgnoreCase)) == true)
        {
            return false;
        }

        if (!userId.HasValue)
        {
            return await _users.OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.EMail == eMail) is null;
        }

        return await _users.OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.EMail == eMail && x.Id != userId.Value) is
            null;
    }

    public async Task<OperationResult> CheckFriendlyNameAsync(string friendlyName, int? userId)
    {
        friendlyName = friendlyName.RemoveDiacritics().NormalizeUnderLines().RemovePunctuation();

        // برای روان پریش‌هایی که اسامی را با فاصله زیاد و تکراری وارد می‌کنند
        var trimmedFriendlyName = friendlyName.Trim().Replace(oldValue: " ", newValue: "", StringComparison.Ordinal);

        if (friendlyName.IsNumeric() || friendlyName.ContainsNumber())
        {
            return ("لطفا در نام مستعار از اعداد استفاده نکنید", OperationStat.Failed);
        }

        if (friendlyName.HasConsecutiveChars())
        {
            return ("لطفا در نام مستعار از حروف تکراری استفاده نکنید", OperationStat.Failed);
        }

        var isValid = !userId.HasValue
            ? await _users.OrderBy(x => x.Id)
                .FirstOrDefaultAsync(x
                    => x.FriendlyName == friendlyName || x.FriendlyName.Replace(" ", "") == trimmedFriendlyName) is null
            : await _users.OrderBy(x => x.Id)
                .FirstOrDefaultAsync(x
                    => (x.FriendlyName == friendlyName || x.FriendlyName.Replace(" ", "") == trimmedFriendlyName) &&
                       x.Id != userId.Value) is null;

        return !isValid
            ? ("نام مستعار وارد شده هم اکنون توسط یکی از کاربران مورد استفاده‌است", OperationStat.Failed)
            : OperationStat.Succeeded;
    }

    public async Task<bool> CheckUsernameAsync(string username, int? userId)
    {
        if (!userId.HasValue)
        {
            return await _users.OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.UserName == username) is null;
        }

        return await _users.OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.UserName == username && x.Id != userId.Value) is null;
    }

    public Task<int> NumberOfNotActivatedUsersAsync(DateTime? from)
        => !from.HasValue
            ? _users.AsNoTracking().CountAsync(user => user.IsActive && !user.EmailIsValidated)
            : _users.AsNoTracking()
                .CountAsync(user => user.IsActive && !user.EmailIsValidated && user.Audit.CreatedAt >= from.Value);

    public async Task<byte[]> GetEmailImageAsync(int? userId)
    {
        var options = new TextToImageOptions();

        if (userId is null)
        {
            return "?".TextToImage(options);
        }

        var user = await FindUserAsync(userId.Value);

        if (user is null)
        {
            return "?".TextToImage(options);
        }

        if (!user.IsEmailPublic)
        {
            return "?".TextToImage(options);
        }

        return user.EMail.TextToImage(options);
    }

    public async Task<User?> FindUserByEMailAsync(string eMail)
    {
        if (string.IsNullOrWhiteSpace(eMail))
        {
            return null;
        }

        eMail = eMail.Trim();
        var user = await _users.OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.EMail == eMail);

        eMail = eMail.FixGmailDots();

        return user ?? await _users.OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.EMail == eMail);
    }

    public Task<List<User>> FindUsersStartWithFriendlyNameAsNoTrackingAsync(string name, int count = 20)
        => _users.AsNoTracking().Where(r => r.FriendlyName.StartsWith(name)).Take(count).ToListAsync();

    public Task<List<User>> FindActiveUsersContainFriendlyNameAsNoTrackingAsync(string name, int count = 20)
        => _users.AsNoTracking().Where(r => r.FriendlyName.Contains(name) && r.IsActive).Take(count).ToListAsync();

    public Task<List<User>> FindUsersContainFriendlyNameAsNoTrackingAsync(string name, int count = 20)
        => _users.AsNoTracking().Where(r => r.FriendlyName.Contains(name)).Take(count).ToListAsync();

    public Task<List<User>> FindUsersContainUserNameAsNoTrackingAsync(string name, int count = 20)
        => _users.AsNoTracking().Where(r => r.UserName.Contains(name)).Take(count).ToListAsync();

    public Task<List<User>> FindUsersContainEMailAsNoTrackingAsync(string name, int count = 20)
        => _users.AsNoTracking().Where(r => r.EMail.Contains(name)).Take(count).ToListAsync();

    public Task<User?> FindUserByFriendlyNameAsync(string name)
        => _users.OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.FriendlyName == name);

    public Task<User?> FindUserByFriendlyNameIncludeUserSocialNetworkAsync(string name)
        => _users.Include(x => x.Roles)
            .Include(x => x.UserSocialNetwork)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.FriendlyName == name);

    public Task<User?> FindUserIncludeUserSocialNetworkAsync(int? id)
        => id is null
            ? Task.FromResult<User?>(result: null)
            : _users.Include(x => x.Roles)
                .Include(x => x.UserSocialNetwork)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(x => x.Id == id.Value);

    public Task<UserSocialNetwork?> FindUserSocialNetworkAsync(int? userid)
        => userid is null
            ? Task.FromResult<UserSocialNetwork?>(result: null)
            : _userUserSocialNetworks.OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.UserId == userid.Value);

    public ValueTask<User?> FindUserAsync(int? userId)
        => userId.HasValue ? _users.FindAsync(userId.Value) : ValueTask.FromResult<User?>(result: null);

    public async Task<User?> FindUserAsync(string username, string password)
    {
        var user = await _users.OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.UserName == username);

        return user is not null && passwordHasherService.IsValidPbkdf2Hash(user.HashedPassword, password) ? user : null;
    }

    public Task<List<User>> FindUsersAsync(IList<int?>? userIds)
    {
        if (userIds is null || userIds.Count == 0)
        {
            return Task.FromResult<List<User>>([]);
        }

        userIds = userIds.Distinct().ToList();

        return _users.AsNoTracking().Where(x => userIds.Contains(x.Id)).ToListAsync();
    }

    public Task<List<User>> GetUsersListByRoleAsync(int count, string role)
    {
        var query = from u in _users
            from i in u.Roles
            where i.Name == role
            select u;

        return query.OrderByDescending(x => x.UserStat.NumberOfPosts)
            .ThenBy(x => x.FriendlyName)
            .Take(count)
            .ToListAsync();
    }

    public Task<List<User>> GetAllWritersWithAtleastOnePostListAsync(int count)
        => _users.Where(x => x.UserStat.NumberOfPosts > 0 && x.IsActive)
            .OrderByDescending(x => x.UserStat.NumberOfPosts)
            .ThenBy(x => x.FriendlyName)
            .Take(count)
            .ToListAsync();

    public Task<PagedResultModel<User>> GetPagedPostsWritersListAsync(int pageNumber, int recordsPerPage)
    {
        var query = _users.AsNoTracking()
            .Where(x => x.UserStat.NumberOfPosts > 0 && x.IsActive)
            .OrderByDescending(x => x.UserStat.NumberOfPosts)
            .ThenBy(x => x.FriendlyName);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<User>> GetPagedLinksWritersListAsync(int pageNumber, int recordsPerPage)
    {
        var query = _users.AsNoTracking()
            .Where(x => x.IsActive && x.UserStat.NumberOfLinks > 0)
            .OrderByDescending(x => x.UserStat.NumberOfLinks)
            .ThenBy(x => x.FriendlyName);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<User>> GetPagedCoursesWritersListAsync(int pageNumber, int recordsPerPage)
    {
        var query = _users.AsNoTracking()
            .Where(x => x.IsActive && x.UserStat.NumberOfCourses > 0)
            .OrderByDescending(x => x.UserStat.NumberOfCourses)
            .ThenBy(x => x.FriendlyName);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<User>> GetPagedProjectsWritersListAsync(int pageNumber, int recordsPerPage)
    {
        var query = _users.AsNoTracking()
            .Where(x => x.IsActive && x.UserStat.NumberOfProjects > 0)
            .OrderByDescending(x => x.UserStat.NumberOfProjects)
            .ThenBy(x => x.FriendlyName);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<User>> GetPagedQuestionsWritersListAsync(int pageNumber, int recordsPerPage)
    {
        var query = _users.AsNoTracking()
            .Where(x => x.IsActive && x.UserStat.NumberOfStackExchangeQuestions > 0)
            .OrderByDescending(x => x.UserStat.NumberOfStackExchangeQuestions)
            .ThenBy(x => x.FriendlyName);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<User>> GetPagedAdvertisementsWritersListAsync(int pageNumber, int recordsPerPage)
    {
        var query = _users.AsNoTracking()
            .Where(x => x.IsActive && x.UserStat.NumberOfAdvertisements > 0)
            .OrderByDescending(x => x.UserStat.NumberOfAdvertisements)
            .ThenBy(x => x.FriendlyName);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<User>> GetPagedBacklogsWritersListAsync(int pageNumber, int recordsPerPage)
    {
        var query = _users.AsNoTracking()
            .Where(x => x.IsActive && x.UserStat.NumberOfBacklogs > 0)
            .OrderByDescending(x => x.UserStat.NumberOfBacklogs)
            .ThenBy(x => x.FriendlyName);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<User>> GetPagedSurveyWritersListAsync(int pageNumber, int recordsPerPage)
    {
        var query = _users.AsNoTracking()
            .Where(x => x.IsActive && x.UserStat.NumberOfSurveys > 0)
            .OrderByDescending(x => x.UserStat.NumberOfSurveys)
            .ThenBy(x => x.FriendlyName);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<User>> GetPagedLearningPathWritersListAsync(int pageNumber, int recordsPerPage)
    {
        var query = _users.AsNoTracking()
            .Where(x => x.IsActive && x.UserStat.NumberOfLearningPaths > 0)
            .OrderByDescending(x => x.UserStat.NumberOfLearningPaths)
            .ThenBy(x => x.FriendlyName);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<List<User>> GetAllActiveWritersListIncludePostsAsync()
    {
        var query = from u in _users
            where u.UserStat.NumberOfPosts > 0 && u.IsActive
            select u;

        return query.Include(x => x.BlogPosts).ToListAsync();
    }

    public Task<List<User>> GetAllActiveUsersListWithMinPostsCountAsync(int minPostCount)
    {
        var query = from u in _users
            where u.UserStat.NumberOfPosts >= minPostCount && u.IsActive
            select u;

        return query.ToListAsync();
    }

    public Task<List<User>> GetAllActiveUsersListWithZeroPostCountAsync()
    {
        var query = from u in _users
            where u.UserStat.NumberOfPosts == 0 && u.IsActive && u.EmailIsValidated
            select u;

        return query.ToListAsync();
    }

    public Task<PagedResultModel<User>> GetUsersListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false,
        bool showAll = true)
    {
        var query = _users.Include(x => x.Roles).Include(x => x.UserSocialNetwork).AsNoTracking();

        if (!showAll)
        {
            query = query.Where(x => x.IsActive);
        }

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<User>> GetUsersListAsNoTrackingAsync(DntQueryBuilderModel state, bool showAll = true)
    {
        var query = _users.Include(x => x.Roles).Include(x => x.UserSocialNetwork).AsNoTracking();

        if (!showAll)
        {
            query = query.Where(x => x.IsActive);
        }

        return query.ApplyQueryableDntGridFilterAsync(state, nameof(User.Id), [
            new GridifyMap<User>
            {
                From = nameof(User.LastVisitDateTime),
                To = user => user.LastVisitDateTime,
                Convertor = persianDateStr => persianDateStr.ToGregorianDateTime() ?? DateTime.UtcNow
            },
            new GridifyMap<User>
            {
                From = nameof(User.EMail),
                To = user => user.EMail,
                Convertor = email => email.FixGmailDots()
            }
        ]);
    }

    public User AddUser(User data) => _users.Add(data).Entity;

    public Task<List<User>> GetActiveLinksWritersListAsync(int count = 1000)
        => _users.Where(user => user.IsActive && user.UserStat.NumberOfLinks > 0)
            .OrderByDescending(x => x.UserStat.NumberOfLinks)
            .ThenBy(x => x.FriendlyName)
            .Take(count)
            .ToListAsync();

    public async Task<List<User>> GetAllDailyEmailReceiversListAsync(int count = 300)
    {
        var nonWriters = await _users.AsNoTracking()
            .Where(user => user.IsActive && user.ReceiveDailyEmails && user.UserStat.NumberOfPosts <= 0 &&
                           user.UserStat.NumberOfLinks <= 0)
            .OrderByDescending(x => x.Rating.TotalRating)
            .Take(count)
            .ToListAsync();

        var allWriters = await _users.AsNoTracking()
            .Where(user => user.IsActive && user.ReceiveDailyEmails &&
                           (user.UserStat.NumberOfPosts > 0 || user.UserStat.NumberOfLinks > 0))
            .ToListAsync();

        return nonWriters.Union(allWriters).ToList();
    }

    public Task<List<User>> GetActiveProjectAuthorsListAsync(int count)
        => _users.Where(user => user.IsActive && user.UserStat.NumberOfProjects > 0)
            .OrderByDescending(x => x.UserStat.NumberOfProjects)
            .ThenBy(x => x.FriendlyName)
            .Take(count)
            .ToListAsync();

    public Task<List<User?>> GetActiveArticleWritersListAsync(int count)
    {
        var query = from u in _users
            where u.UserStat.NumberOfPosts > 0 && u.IsActive
            select u;

        return query.OrderByDescending(x => x.UserStat.NumberOfPosts)
            .ThenBy(x => x.FriendlyName)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<User?>> GetActiveArticleWritersListAsync(int count, DateTime fromDate, DateTime toDate)
    {
        var activeUsersFromToFrom = await GetActiveAuthorUsersFromToFromAsync(count, fromDate, toDate);

        var activeCourseAuthorUsersFromToFrom =
            await GetActiveCourseAuthorUsersFromToFromAsync(count, fromDate, toDate);

        var activeLearningPathUsersFromToFrom =
            await GetActiveLearningPathAuthorUsersFromToFromAsync(count, fromDate, toDate);

        var activeProjectsUsersFromToFrom = await GetActiveProjectsAuthorUsersFromToFromAsync(count, fromDate, toDate);
        var activeLinksUsersFromToFrom = await GetActiveLinksAuthorUsersFromToFromAsync(count, fromDate, toDate);

        var users = activeUsersFromToFrom.Union(activeCourseAuthorUsersFromToFrom)
            .Union(activeLearningPathUsersFromToFrom)
            .Union(activeProjectsUsersFromToFrom)
            .Union(activeLinksUsersFromToFrom)
            .ToList();

        return users.Count == 0
            ? await GetActiveArticleWritersListAsync(count)
            : users.Distinct()
                .OrderByDescending(x => x!.UserStat.NumberOfPosts)
                .ThenBy(x => x!.FriendlyName)
                .Take(count)
                .ToList();
    }

    public Task<List<User>> GetJobSeekersListAsNoTrackingAsync()
    {
        var query = _users.Where(x => x.IsActive && x.IsJobsSeeker)
            .AsNoTracking()
            .Include(x => x.Roles)
            .Include(x => x.UserSocialNetwork);

        return query.OrderByDescending(x => x.UserStat.NumberOfPosts).ThenBy(x => x.FriendlyName).ToListAsync();
    }

    public Task<int> WriterNumberOfPostsAsync(int userId,
        DateTime fromDate,
        DateTime toDate,
        bool userIsActive = true,
        bool showDeletedItems = false)
        => _users.AsNoTracking()
            .Where(x => x.IsActive == userIsActive && x.Id == userId)
            .SelectMany(x => x.BlogPosts)
            .CountAsync(x
                => x.IsDeleted == showDeletedItems && x.Audit.CreatedAt >= fromDate && x.Audit.CreatedAt <= toDate);

    public Task<List<User>> GetAllActiveAdminsAsNoTrackingAsync()
        => (from u in _users.AsNoTracking()
            where u.IsActive
            from r in u.Roles
            where r.Name == CustomRoles.Admin
            select u).ToListAsync();

    public async Task<List<User>> GetNotLoggedInUsersToDisableAsync(int month)
    {
        var limit = DateTime.UtcNow.AddMonths(-month);

        return await _users.Where(user => user.IsActive &&
                                          user.EmailIsValidated && user.UserStat.NumberOfPosts == 0 &&
                                          (user.LastVisitDateTime == null || user.LastVisitDateTime < limit))
            .OrderBy(user => user.Id)
            .ToListAsync();
    }

    private Task<List<User?>> GetActiveLinksAuthorUsersFromToFromAsync(int count, DateTime fromDate, DateTime toDate)
        => _users.Where(x => x.IsActive)
            .SelectMany(x => x.DailyNewsItems)
            .Where(x => !x.IsDeleted && x.Audit.CreatedAt >= fromDate && x.Audit.CreatedAt <= toDate)
            .Select(x => x.User)
            .Distinct()
            .OrderByDescending(x => x!.UserStat.NumberOfPosts)
            .ThenBy(x => x!.FriendlyName)
            .Take(count)
            .ToListAsync();

    private Task<List<User?>> GetActiveProjectsAuthorUsersFromToFromAsync(int count, DateTime fromDate, DateTime toDate)
        => _users.Where(x => x.IsActive)
            .SelectMany(x => x.Projects)
            .Where(x => !x.IsDeleted && x.Audit.CreatedAt >= fromDate && x.Audit.CreatedAt <= toDate)
            .Select(x => x.User)
            .Distinct()
            .OrderByDescending(x => x!.UserStat.NumberOfPosts)
            .ThenBy(x => x!.FriendlyName)
            .Take(count)
            .ToListAsync();

    private Task<List<User?>>
        GetActiveLearningPathAuthorUsersFromToFromAsync(int count, DateTime fromDate, DateTime toDate)
        => _users.Where(x => x.IsActive)
            .SelectMany(x => x.LearningPaths)
            .Where(x => !x.IsDeleted && x.Audit.CreatedAt >= fromDate && x.Audit.CreatedAt <= toDate)
            .Select(x => x.User)
            .Distinct()
            .OrderByDescending(x => x!.UserStat.NumberOfPosts)
            .ThenBy(x => x!.FriendlyName)
            .Take(count)
            .ToListAsync();

    private Task<List<User?>> GetActiveCourseAuthorUsersFromToFromAsync(int count, DateTime fromDate, DateTime toDate)
        => _users.Where(x => x.IsActive)
            .SelectMany(x => x.CourseTopics)
            .Where(x => x.Audit.CreatedAt >= fromDate && x.Audit.CreatedAt <= toDate)
            .Select(x => x.User)
            .Distinct()
            .OrderByDescending(x => x!.UserStat.NumberOfPosts)
            .ThenBy(x => x!.FriendlyName)
            .Take(count)
            .ToListAsync();

    private Task<List<User?>> GetActiveAuthorUsersFromToFromAsync(int count, DateTime fromDate, DateTime toDate)
        => _users.Where(x => x.IsActive)
            .SelectMany(x => x.BlogPosts)
            .Where(x => !x.IsDeleted && x.Audit.CreatedAt >= fromDate && x.Audit.CreatedAt <= toDate)
            .Select(x => x.User)
            .Distinct()
            .OrderByDescending(x => x!.UserStat.NumberOfPosts)
            .ThenBy(x => x!.FriendlyName)
            .Take(count)
            .ToListAsync();
}
