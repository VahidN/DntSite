using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.RoadMaps.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.RoadMaps.Services.Contracts;

public interface ILearningPathService : IScopedService
{
    public ValueTask<LearningPath?> FindLearningPathAsync(int id);

    public Task<LearningPath?> GetLearningPathAsync(int id, bool showDeletedItems = false);

    public IList<int> GetItemPostIds(string contains, LearningPath learningPathItem, string domain);

    public LearningPath AddLearningPath(LearningPath data);

    public Task<PagedResultModel<LearningPath>> GetLearningPathsAsync(int pageNumber,
        int? userId = null,
        int recordsPerPage = 15,
        bool showAll = false,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<LearningPath>> GetLastPagedLearningPathsAsync(DntQueryBuilderModel state,
        bool showDeletedItems = false,
        bool showAll = false);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Task<LearningPathDetailsModel> LearningPathDetailsAsync(int id, bool showDeletedItems = false);

    public Task<List<LearningPath>> GetAllPublicLearningPathsOfDateAsync(DateTime date);

    public string GetTagPdfFileName(LearningPath tag, string fileName = "dot-net-tips-learning-path-");

    public Task UpdateStatAsync(int learningPathId, bool isFromFeed);

    public Task<PagedResultModel<LearningPath>> GetLearningPathsByTagNameAsync(string tagName,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<LearningPath>> GetLastPagedLearningPathsOfUserAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        bool showAll = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task MarkAsDeletedAsync(LearningPath? learningPathItem);

    public Task NotifyDeleteChangesAsync(LearningPath? learningPathItem);

    public Task UpdateLearningPathAsync(LearningPath? learningPathItem, LearningPathModel writeLearningPathModel);

    public Task<LearningPath?> AddLearningPathAsync(LearningPathModel writeLearningPathModel, User? user);

    public Task NotifyAddOrUpdateChangesAsync(LearningPath? learningPathItem);

    public Task IndexLearningPathsAsync();
}
