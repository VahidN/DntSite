using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.PrivateMessages.Entities;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.PrivateMessages.Services;

public class MassEmailsService(IUnitOfWork uow, IAntiXssService antiXssService) : IMassEmailsService
{
    private readonly DbSet<MassEmail> _massEmails = uow.DbSet<MassEmail>();

    public async Task<string> AddMassEmailAsync(MassEmailModel data, int userId)
    {
        ArgumentNullException.ThrowIfNull(data);

        var body = antiXssService.GetSanitizedHtml(data.NewsBody);

        _massEmails.Add(new MassEmail
        {
            IsDeleted = false,
            NewsBody = body,
            NewsTitle = data.NewsTitle.Trim(),
            UserId = userId,
            EmailsSent = true
        });

        await uow.SaveChangesAsync();

        return body;
    }
}
