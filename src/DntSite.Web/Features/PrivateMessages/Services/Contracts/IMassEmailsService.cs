using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.PrivateMessages.Services.Contracts;

public interface IMassEmailsService : IScopedService
{
    Task<string> AddMassEmailAsync(MassEmailModel? data, int userId);
}
