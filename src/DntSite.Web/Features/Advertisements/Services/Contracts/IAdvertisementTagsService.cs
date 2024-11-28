using DntSite.Web.Features.Advertisements.Entities;

namespace DntSite.Web.Features.Advertisements.Services.Contracts;

public interface IAdvertisementTagsService : IScopedService
{
    public Task<string> AvailableTagsToJsonAsync(int count = 1000);

    public Task SaveNewAdvertisementTagsAsync(string[] tagsList, string userName);

    public Task<List<AdvertisementTag>> FindListOfActualAdvertisementTagsAsync(string[] tagsList);

    public Task<List<AdvertisementTag>> GetThisAdvertisementTagsListAsync(int id);

    public Task<List<AdvertisementTag>> GetAllAdvertisementTagsListAsNoTrackingAsync(int count);

    public ValueTask<AdvertisementTag?> FindAdvertisementTagAsync(int tagId);
}
