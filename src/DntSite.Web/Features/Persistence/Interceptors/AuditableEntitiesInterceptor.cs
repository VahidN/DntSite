using DntSite.Web.Features.Common.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.Utils;
using DntSite.Web.Features.UserProfiles.Services;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DntSite.Web.Features.Persistence.Interceptors;

public class AuditableEntitiesInterceptor(
    IHttpContextAccessor httpContextAccessor,
    ILogger<AuditableEntitiesInterceptor> logger) : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor =
        httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    private readonly ILogger<AuditableEntitiesInterceptor> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData?.Context is null)
        {
            return result;
        }

        BeforeSaveTriggers(eventData.Context);

        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData?.Context is null)
        {
            return ValueTask.FromResult(result);
        }

        BeforeSaveTriggers(eventData.Context);

        return ValueTask.FromResult(result);
    }

    private void BeforeSaveTriggers(DbContext context)
    {
        ValidateEntities(context);
        ApplyAudits(context.ChangeTracker);
    }

    private void ValidateEntities(DbContext context)
    {
        var errors = context.GetValidationErrors();

        if (string.IsNullOrWhiteSpace(errors))
        {
            return;
        }

        _logger.LogErrorMessage(errors);

        throw new InvalidOperationException(errors);
    }

    private void ApplyAudits(ChangeTracker? changeTracker)
    {
        if (changeTracker is null)
        {
            return;
        }

        foreach (var entry in changeTracker.Entries<BaseEntity>())
        {
            var auditAction = CreateAuditAction();

            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.UserId ??= auditAction.IdentityName;

                    entry.Entity.Audit ??= new AuditBase();
                    entry.Entity.Audit.CreatedByUserAgent ??= auditAction.CreatedByUserAgent;
                    entry.Entity.Audit.CreatedByUserIp ??= auditAction.CreatedByUserIp;
                    entry.Entity.Audit.CreatedAt = auditAction.CreatedAt;
                    entry.Entity.Audit.CreatedAtPersian = auditAction.CreatedAtPersian;

                    entry.Entity.GuestUser.UserName ??= GuestUserUserName(entry);

                    entry.Entity.GuestUser.Email ??= entry.Entity.User?.EMail;

                    auditAction.Action = AuditActionType.Created;
                    UpdateAuditsFieldsValue(entry, auditAction);

                    break;
                case EntityState.Modified:
                    auditAction.Action = AuditActionType.Edited;
                    UpdateAuditsFieldsValue(entry, auditAction);

                    break;
                case EntityState.Deleted:
                    if (IgnoreSoftDelete(entry.Metadata))
                    {
                        continue;
                    }

                    // NOTE: This part converts the original `Remove()` method to a soft-delete action.
                    // TIP: To cascade-delete all the related entities, you should `Include` them in your find method and then remove the whole entity.
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;

                    auditAction.Action = AuditActionType.Deleted;
                    UpdateAuditsFieldsValue(entry, auditAction);

                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                default:
                    continue;
            }
        }
    }

    private string GuestUserUserName(EntityEntry<BaseEntity> entry)
    {
        var friendlyName = entry.Entity.User?.FriendlyName;

        if (!string.IsNullOrWhiteSpace(friendlyName))
        {
            return friendlyName;
        }

        var displayName =
            _httpContextAccessor.HttpContext?.User.GetDisplayName(UserRolesService.DisplayNameClaim, defaultValue: "");

        return !string.IsNullOrWhiteSpace(displayName) ? displayName : SharedConstants.GuestUserName;
    }

    private AuditAction CreateAuditAction()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var utcNow = DateTime.UtcNow;

        return new AuditAction
        {
            IdentityName = httpContext?.User?.GetUserId(),
            CreatedAt = utcNow,
            CreatedAtPersian = utcNow.ToShortPersianDateTimeString(),
            CreatedByUserIp = httpContext?.GetIP() ?? "::1",
            CreatedByUserAgent = httpContext?.GetUserAgent() ?? "Program"
        };
    }

    private static void UpdateAuditsFieldsValue(EntityEntry<BaseEntity> entry, AuditAction auditField)
    {
        if (entry.Entity is not BaseAuditedEntity baseAuditedEntity)
        {
            return;
        }

        var hasAffectedColumns = false;

        foreach (var property in entry.Properties)
        {
            if (IgnoreAuditProperty(property.Metadata))
            {
                continue;
            }

            if (property.Metadata.IsPrimaryKey() || property.IsTemporary)
            {
                // We don't want to store the auto-generated ID of this row again,
                // because we have it in a separate field and
                // we are going to store this audit JSON-column in this row.
                continue;
            }

            var currentValue = JsonSerializer.Serialize(property.CurrentValue);
            var originalValue = JsonSerializer.Serialize(property.OriginalValue);

            if (auditField.Action == AuditActionType.Created || (property.IsModified &&
                                                                 !string.Equals(originalValue, currentValue,
                                                                     StringComparison.Ordinal)))
            {
                auditField.AffectedColumns.Add(new AffectedColumn
                {
                    Name = property.Metadata.Name,
                    Value = currentValue
                });

                hasAffectedColumns = true;
            }
        }

        if (hasAffectedColumns)
        {
            baseAuditedEntity.AuditActions.Add(auditField);
        }
    }

    private static bool IgnoreAuditProperty(IReadOnlyPropertyBase propertyMetadata)
    {
        var memberInfo = propertyMetadata.PropertyInfo ?? propertyMetadata.FieldInfo as MemberInfo;

        return memberInfo is not null && Attribute.IsDefined(memberInfo, typeof(IgnoreAuditAttribute), inherit: true);
    }

    private static bool IgnoreSoftDelete(IReadOnlyTypeBase entryMetadata)
        => Attribute.IsDefined(entryMetadata.ClrType, typeof(IgnoreSoftDeleteAttribute), inherit: true);
}
