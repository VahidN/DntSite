using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.RoadMaps.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.RoadMaps.Services.Contracts;

public interface ILearningPathService : IScopedService
{
    ValueTask<LearningPath?> FindLearningPathAsync(int id);

    Task<LearningPath?> GetLearningPathAsync(int id, bool showDeletedItems = false);

    LearningPath AddLearningPath(LearningPath data);

    Task<PagedResultModel<LearningPath>> GetLearningPathsAsync(int pageNumber,
        int? userId = null,
        int recordsPerPage = 15,
        bool showAll = false,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<LearningPath>> GetLastPagedLearningPathsAsync(DntQueryBuilderModel state,
        bool showDeletedItems = false,
        bool showAll = false);

    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    Task<LearningPathDetailsModel> LearningPathDetailsAsync(int id, bool showDeletedItems = false);

    Task<List<LearningPath>> GetAllPublicLearningPathsOfDateAsync(DateTime date);

    string GetTagPdfFileName(LearningPath tag, string fileName = "dot-net-tips-learning-path-");

    Task UpdateStatAsync(int learningPathId, bool isFromFeed);

    Task<PagedResultModel<LearningPath>> GetLearningPathsByTagNameAsync(string tagName,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<LearningPath>> GetLastPagedLearningPathsOfUserAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        bool showAll = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task MarkAsDeletedAsync(LearningPath? learningPathItem);

    Task NotifyDeleteChangesAsync(LearningPath? learningPathItem);

    Task UpdateLearningPathAsync(LearningPath? learningPathItem, LearningPathModel? writeLearningPathModel);

    Task<LearningPath?> AddLearningPathAsync(LearningPathModel? writeLearningPathModel, User? user);

    Task NotifyAddOrUpdateChangesAsync(LearningPath? learningPathItem);

    Task IndexLearningPathsAsync();
}
