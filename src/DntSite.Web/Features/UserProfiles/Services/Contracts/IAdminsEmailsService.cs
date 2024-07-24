using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface IAdminsEmailsService : IScopedService
{
    Task UploadFileSendEmailAsync(string path, string actionUrl);

    Task TagEditedSendEmailAsync<TLayout, TLayoutModel>(TLayoutModel data)
        where TLayout : IComponent
        where TLayoutModel : BaseEmailModel;

    Task CommonFileEditedSendEmailAsync(string name, string description);

    Task PostNewReferrersEmailAsync(Uri destUri, Uri sourceUri);

    Task SendRecycleEmailAsync(string id, string title, string body);
}
