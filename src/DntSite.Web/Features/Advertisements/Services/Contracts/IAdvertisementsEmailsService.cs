using DntSite.Web.Features.Advertisements.Entities;

namespace DntSite.Web.Features.Advertisements.Services.Contracts;

public interface IAdvertisementsEmailsService : IScopedService
{
    public Task AddAdvertisementSendEmailAsync(Advertisement advertisement);

    public Task AdvertisementCommentSendEmailToWritersAsync(AdvertisementComment comment);

    public Task AdvertisementCommentSendEmailToPersonAsync(AdvertisementComment comment);

    public Task AdvertisementCommentSendEmailToAdminsAsync(AdvertisementComment comment);
}
