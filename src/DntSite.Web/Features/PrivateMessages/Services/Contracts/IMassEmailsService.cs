using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.PrivateMessages.Services.Contracts;

public interface IMassEmailsService : IScopedService
{
    public Task<string> AddMassEmailAsync(MassEmailModel data, int userId);
}
