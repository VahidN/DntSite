using DntSite.Web.Features.RoadMaps.Entities;

namespace DntSite.Web.Features.RoadMaps.Models;

public class LearningPathDetailsModel
{
    public LearningPath? CurrentItem { set; get; }

    public LearningPath? NextItem { set; get; }

    public LearningPath? PreviousItem { set; get; }
}
