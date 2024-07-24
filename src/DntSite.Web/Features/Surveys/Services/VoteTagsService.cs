using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.Services.Contracts;

namespace DntSite.Web.Features.Surveys.Services;

public class VoteTagsService(IUnitOfWork uow) : IVoteTagsService
{
    private readonly DbSet<SurveyTag> _voteTags = uow.DbSet<SurveyTag>();

    public Task<List<SurveyTag>> GetAllVoteTagsListAsNoTrackingAsync(int count)
        => _voteTags.AsNoTracking().OrderByDescending(x => x.InUseCount).ThenBy(x => x.Name).Take(count).ToListAsync();

    public ValueTask<SurveyTag?> FindVoteTagAsync(int tagId) => _voteTags.FindAsync(tagId);
}
