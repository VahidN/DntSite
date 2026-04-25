namespace DntSite.Web.Features.Exports.Models;

public record EPubContentItem(
    int Id,
    string Title,
    string? Content,
    Guid? DisplayId,
    string? Author,
    DateTime? PublishDate,
    string? Url);
