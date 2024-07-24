using DntSite.Web.Features.Advertisements.Entities;

namespace DntSite.Web.Features.Advertisements.Services.Contracts;

public interface IAdvertisementTagsService : IScopedService
{
    Task<string> AvailableTagsToJsonAsync(int count = 1000);

    Task SaveNewAdvertisementTagsAsync(string[] tagsList, string userName);

    Task<List<AdvertisementTag>> FindListOfActualAdvertisementTagsAsync(string[] tagsList);

    Task<List<AdvertisementTag>> GetThisAdvertisementTagsListAsync(int id);

    Task<List<AdvertisementTag>> GetAllAdvertisementTagsListAsNoTrackingAsync(int count);

    ValueTask<AdvertisementTag?> FindAdvertisementTagAsync(int tagId);
}
