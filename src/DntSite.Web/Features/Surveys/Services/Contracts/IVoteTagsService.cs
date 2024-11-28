using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.Services.Contracts;

public interface IVoteTagsService : IScopedService
{
    public Task<List<SurveyTag>> GetAllVoteTagsListAsNoTrackingAsync(int count);

    public ValueTask<SurveyTag?> FindVoteTagAsync(int tagId);
}
