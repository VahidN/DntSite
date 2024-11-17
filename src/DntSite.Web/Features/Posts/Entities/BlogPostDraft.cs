using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Posts.Entities;

public class BlogPostDraft : BaseAuditedEntity
{
    public required string Title { set; get; }

    [MaxLength] public required string Body { set; get; }

    public int ReadingTimeMinutes { set; get; }

    [Required] public required IList<string> Tags { set; get; } = [];

    public bool IsReady { set; get; }

    public DateTime? DateTimeToShow { set; get; }

    public int NumberOfRequiredPoints { set; get; }

    public bool IsConverted { set; get; }
}
