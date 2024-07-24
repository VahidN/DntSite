using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Common.ModelsMappings;

public static class GridifyMapings
{
    public static IList<GridifyMap<TEntity>> GetDefaultMappings<TEntity>()
        where TEntity : BaseAuditedEntity
        =>
        [
            new GridifyMap<TEntity>
            {
                From = "User_FriendlyName",
                To = entity => entity.User!.FriendlyName
            },
            new GridifyMap<TEntity>
            {
                From = "Audit_CreatedAtPersian",
                To = entity => entity.Audit.CreatedAtPersian
            }
        ];
}
