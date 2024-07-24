using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.Services.Contracts;

public interface IVoteTagsService : IScopedService
{
    Task<List<SurveyTag>> GetAllVoteTagsListAsNoTrackingAsync(int count);

    ValueTask<SurveyTag?> FindVoteTagAsync(int tagId);
}
