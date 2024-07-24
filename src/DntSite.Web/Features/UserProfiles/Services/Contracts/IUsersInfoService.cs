using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface IUsersInfoService : IScopedService
{
    Task<bool> IsWriterAsync(int userId);

    Task<bool> CheckEMailAsync(string eMail, int? userId);

    Task<OperationResult> CheckFriendlyNameAsync(string friendlyName, int? userId);

    Task<bool> CheckUsernameAsync(string username, int? userId);

    Task<int> NumberOfNotActivatedUsersAsync(DateTime? from);

    Task<byte[]> GetEmailImageAsync(int? userId);

    Task<User?> FindUserByEMailAsync(string eMail);

    Task<List<User>> FindUsersStartWithFriendlyNameAsNoTrackingAsync(string name, int count = 20);

    Task<List<User>> FindActiveUsersContainFriendlyNameAsNoTrackingAsync(string name, int count = 20);

    Task<List<User>> FindUsersContainFriendlyNameAsNoTrackingAsync(string name, int count = 20);

    Task<List<User>> FindUsersContainUserNameAsNoTrackingAsync(string name, int count = 20);

    Task<List<User>> FindUsersContainEMailAsNoTrackingAsync(string name, int count = 20);

    Task<User?> FindUserByFriendlyNameAsync(string name);

    Task<User?> FindUserByFriendlyNameIncludeUserSocialNetworkAsync(string name);

    Task<User?> FindUserIncludeUserSocialNetworkAsync(int? id);

    Task<UserSocialNetwork?> FindUserSocialNetworkAsync(int? userid);

    ValueTask<User?> FindUserAsync(int? userId);

    Task<User?> FindUserAsync(string username, string password);

    Task<List<User>> GetUsersListByRoleAsync(int count, string role);

    Task<List<User>> GetAllWritersWithAtleastOnePostListAsync(int count);

    Task<PagedResultModel<User>> GetPagedPostsWritersListAsync(int pageNumber, int recordsPerPage);

    Task<PagedResultModel<User>> GetPagedLinksWritersListAsync(int pageNumber, int recordsPerPage);

    Task<PagedResultModel<User>> GetPagedProjectsWritersListAsync(int pageNumber, int recordsPerPage);

    Task<PagedResultModel<User>> GetPagedQuestionsWritersListAsync(int pageNumber, int recordsPerPage);

    Task<PagedResultModel<User>> GetPagedAdvertisementsWritersListAsync(int pageNumber, int recordsPerPage);

    Task<PagedResultModel<User>> GetPagedBacklogsWritersListAsync(int pageNumber, int recordsPerPage);

    Task<PagedResultModel<User>> GetPagedSurveyWritersListAsync(int pageNumber, int recordsPerPage);

    Task<PagedResultModel<User>> GetPagedLearningPathWritersListAsync(int pageNumber, int recordsPerPage);

    Task<List<User>> GetAllActiveWritersListIncludePostsAsync();

    Task<List<User>> GetAllActiveUsersListWithMinPostsCountAsync(int minPostCount);

    Task<List<User>> GetAllActiveUsersListWithZeroPostCountAsync();

    Task<PagedResultModel<User>> GetUsersListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false,
        bool showAll = true);

    Task<PagedResultModel<User>> GetUsersListAsNoTrackingAsync(DntQueryBuilderModel state, bool showAll = true);

    User AddUser(User data);

    Task<User?> EditUserRolesAsync(string[] roles, int userId);

    Task<List<User>> GetActiveLinksWritersListAsync(int count = 1000);

    Task<List<User>> GetAllDailyEmailReceiversListAsync(int count = 300);

    Task<List<User>> GetActiveProjectAuthorsListAsync(int count);

    Task<List<User?>> GetActiveArticleWritersListAsync(int count);

    Task<List<User?>> GetActiveArticleWritersListAsync(int count, DateTime fromDate, DateTime toDate);

    Task<List<User>> GetJobSeekersListAsNoTrackingAsync();

    Task<int> WriterNumberOfPostsAsync(int userId,
        DateTime fromDate,
        DateTime toDate,
        bool userIsActive = true,
        bool showDeletedItems = false);

    Task<List<User>> GetAllActiveAdminsAsNoTrackingAsync();
}
