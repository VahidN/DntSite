using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.RoadMaps.Models;

public class LearningPathToAdminEmailModel : BaseEmailModel
{
    public required string Id { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required string Author { get; set; }

    public required string Stat { get; set; }
}
