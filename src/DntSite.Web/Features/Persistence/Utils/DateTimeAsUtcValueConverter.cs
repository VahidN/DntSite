using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DntSite.Web.Features.Persistence.Utils;

/// <summary>
///     If you store a DateTime object to the DB with a DateTimeKind of either `Utc` or `Local`,
///     when you read that record back from the DB you'll get a DateTime object whose kind is `Unspecified`.
///     Here is a fix for it!
/// </summary>
public class DateTimeAsUtcValueConverter() : ValueConverter<DateTime, DateTime>(
    v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
