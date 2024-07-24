using System.Text;

namespace DntSite.Web.Features.Persistence.Utils;

public static class EfCorerExtensions
{
    public static string GetValidationErrors(this DbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var errors = new StringBuilder();

        var entities = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .Select(e => e.Entity);

        foreach (var entity in entities)
        {
            var validationContext = new ValidationContext(entity);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(entity, validationContext, validationResults, true))
            {
                foreach (var validationResult in validationResults)
                {
                    var names = validationResult.MemberNames.Aggregate((s1, s2) => $"{s1}, {s2}");
                    errors.AppendFormat(CultureInfo.InvariantCulture, "{0}: {1}", names, validationResult.ErrorMessage);
                }
            }
        }

        return errors.ToString();
    }

    public static void RejectChanges(this DbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        foreach (var entry in context.ChangeTracker.Entries())
        {
            entry.State = entry.State switch
            {
                EntityState.Modified => EntityState.Unchanged,
                EntityState.Added => EntityState.Detached,
                _ => entry.State
            };
        }
    }

    public static string? GetSchemaQualifiedTableName<T>(this DbContext context)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(context);

        var entityType = context.Model.FindEntityType(typeof(T));

        return entityType?.GetSchemaQualifiedTableName();
    }
}
