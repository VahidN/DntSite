using AutoMapper;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.RoadMaps.Models;

namespace DntSite.Web.Features.RoadMaps.ModelsMappings;

public class AfterMapLearningPathModel(IAppAntiXssService antiXssService)
    : IMappingAction<LearningPathModel, LearningPath>
{
    public void Process(LearningPathModel source, LearningPath destination, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.Description = antiXssService.GetSanitizedHtml(source.Description);
    }
}
