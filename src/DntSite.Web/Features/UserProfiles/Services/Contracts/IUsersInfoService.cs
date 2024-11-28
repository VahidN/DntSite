using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface IUsersInfoService : IScopedService
{
    public Task<List<User>> GetNotLoggedInUsersToDisableAsync(int month);

    public Task<bool> IsWriterAsync(int userId);

    public Task<bool> CheckEMailAsync(string eMail, int? userId);

    public Task<OperationResult> CheckFriendlyNameAsync(string friendlyName, int? userId);

    public Task<bool> CheckUsernameAsync(string username, int? userId);

    public Task<int> NumberOfNotActivatedUsersAsync(DateTime? from);

    public Task<byte[]> GetEmailImageAsync(int? userId);

    public Task<User?> FindUserByEMailAsync(string eMail);

    public Task<List<User>> FindUsersAsync(IList<int?>? userIds);

    public Task<List<User>> FindUsersStartWithFriendlyNameAsNoTrackingAsync(string name, int count = 20);

    public Task<List<User>> FindActiveUsersContainFriendlyNameAsNoTrackingAsync(string name, int count = 20);

    public Task<List<User>> FindUsersContainFriendlyNameAsNoTrackingAsync(string name, int count = 20);

    public Task<List<User>> FindUsersContainUserNameAsNoTrackingAsync(string name, int count = 20);

    public Task<List<User>> FindUsersContainEMailAsNoTrackingAsync(string name, int count = 20);

    public Task<User?> FindUserByFriendlyNameAsync(string name);

    public Task<User?> FindUserByFriendlyNameIncludeUserSocialNetworkAsync(string name);

    public Task<User?> FindUserIncludeUserSocialNetworkAsync(int? id);

    public Task<UserSocialNetwork?> FindUserSocialNetworkAsync(int? userid);

    public ValueTask<User?> FindUserAsync(int? userId);

    public Task<User?> FindUserAsync(string username, string password);

    public Task<List<User>> GetUsersListByRoleAsync(int count, string role);

    public Task<List<User>> GetAllWritersWithAtleastOnePostListAsync(int count);

    public Task<PagedResultModel<User>> GetPagedPostsWritersListAsync(int pageNumber, int recordsPerPage);

    public Task<PagedResultModel<User>> GetPagedLinksWritersListAsync(int pageNumber, int recordsPerPage);

    public Task<PagedResultModel<User>> GetPagedCoursesWritersListAsync(int pageNumber, int recordsPerPage);

    public Task<PagedResultModel<User>> GetPagedProjectsWritersListAsync(int pageNumber, int recordsPerPage);

    public Task<PagedResultModel<User>> GetPagedQuestionsWritersListAsync(int pageNumber, int recordsPerPage);

    public Task<PagedResultModel<User>> GetPagedAdvertisementsWritersListAsync(int pageNumber, int recordsPerPage);

    public Task<PagedResultModel<User>> GetPagedBacklogsWritersListAsync(int pageNumber, int recordsPerPage);

    public Task<PagedResultModel<User>> GetPagedSurveyWritersListAsync(int pageNumber, int recordsPerPage);

    public Task<PagedResultModel<User>> GetPagedLearningPathWritersListAsync(int pageNumber, int recordsPerPage);

    public Task<List<User>> GetAllActiveWritersListIncludePostsAsync();

    public Task<List<User>> GetAllActiveUsersListWithMinPostsCountAsync(int minPostCount);

    public Task<List<User>> GetAllActiveUsersListWithZeroPostCountAsync();

    public Task<PagedResultModel<User>> GetUsersListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false,
        bool showAll = true);

    public Task<PagedResultModel<User>> GetUsersListAsNoTrackingAsync(DntQueryBuilderModel state, bool showAll = true);

    public User AddUser(User data);

    public Task<List<User>> GetActiveLinksWritersListAsync(int count = 1000);

    public Task<List<User>> GetAllDailyEmailReceiversListAsync(int count = 300);

    public Task<List<User>> GetActiveProjectAuthorsListAsync(int count);

    public Task<List<User?>> GetActiveArticleWritersListAsync(int count);

    public Task<List<User?>> GetActiveArticleWritersListAsync(int count, DateTime fromDate, DateTime toDate);

    public Task<List<User>> GetJobSeekersListAsNoTrackingAsync();

    public Task<int> WriterNumberOfPostsAsync(int userId,
        DateTime fromDate,
        DateTime toDate,
        bool userIsActive = true,
        bool showDeletedItems = false);

    public Task<List<User>> GetAllActiveAdminsAsNoTrackingAsync();
}
