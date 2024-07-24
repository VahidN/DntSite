using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Advertisements.Services.Contracts;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;

namespace DntSite.Web.Features.Advertisements.Services;

public class AdvertisementTagsService(IUnitOfWork uow, ITagsService tagsService) : IAdvertisementTagsService
{
    private readonly DbSet<Advertisement> _advertisement = uow.DbSet<Advertisement>();
    private readonly DbSet<AdvertisementTag> _advertisementTags = uow.DbSet<AdvertisementTag>();

    public Task<string> AvailableTagsToJsonAsync(int count = 1000) => tagsService.AvailableTagsToJsonAsync(count);

    public async Task SaveNewAdvertisementTagsAsync(string[] tagsList, string userName)
    {
        tagsList = tagsList.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.GetCleanedTag()).ToArray()!;

        var listOfActualTagNames = await _advertisementTags.Where(x => tagsList.Contains(x.Name))
            .Select(x => x.Name)
            .ToListAsync();

        foreach (var tag in tagsList)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                continue;
            }

            if (!listOfActualTagNames.Contains(tag.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                _advertisementTags.Add(new AdvertisementTag
                {
                    Name = tag.GetCleanedTag()!
                });
            }
        }

        await uow.SaveChangesAsync();
    }

    public Task<List<AdvertisementTag>> FindListOfActualAdvertisementTagsAsync(string[] tagsList)
    {
        tagsList = tagsList.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.GetCleanedTag()).ToArray()!;

        return _advertisementTags.AsNoTracking().Where(x => tagsList.Contains(x.Name)).ToListAsync();
    }

    public Task<List<AdvertisementTag>> GetThisAdvertisementTagsListAsync(int id)
        => _advertisement.AsNoTracking()
            .Include(x => x.Tags)
            .Where(x => x.Id == id)
            .SelectMany(x => x.Tags)
            .ToListAsync();

    public Task<List<AdvertisementTag>> GetAllAdvertisementTagsListAsNoTrackingAsync(int count)
        => _advertisementTags.AsNoTracking()
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name)
            .Take(count)
            .ToListAsync();

    public ValueTask<AdvertisementTag?> FindAdvertisementTagAsync(int tagId) => _advertisementTags.FindAsync(tagId);
}
