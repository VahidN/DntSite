using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface IAdminsEmailsService : IScopedService
{
    public Task UploadFileSendEmailAsync(string path, string actionUrl, string formattedFileSize);

    public Task TagEditedSendEmailAsync<TLayout, TLayoutModel>(TLayoutModel data)
        where TLayout : IComponent
        where TLayoutModel : BaseEmailModel;

    public Task CommonFileEditedSendEmailAsync(string name, string description);

    public Task PostNewReferrersEmailAsync(Uri destUri, Uri sourceUri);

    public Task SendRecycleEmailAsync(string id, string title, string body);
}
