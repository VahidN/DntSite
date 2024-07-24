namespace DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class IgnoreAuditAttribute : Attribute
{
    // NOTE: If you don't want it on an specific type,
    // Just ignore it by using the ef-core's entities settings
    // builder.Ignore(entity => entity.Audits)
}
