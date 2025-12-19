using DntSite.Web.Features.Advertisements.Entities;

namespace DntSite.Web.Features.Advertisements.Services.Contracts;

public interface IAdvertisementsEmailsService : IScopedService
{
    Task AddAdvertisementSendEmailAsync(Advertisement advertisement);

    Task AdvertisementCommentSendEmailToWritersAsync(AdvertisementComment comment);

    Task AdvertisementCommentSendEmailToPersonAsync(AdvertisementComment comment);

    Task AdvertisementCommentSendEmailToAdminsAsync(AdvertisementComment comment);
}
