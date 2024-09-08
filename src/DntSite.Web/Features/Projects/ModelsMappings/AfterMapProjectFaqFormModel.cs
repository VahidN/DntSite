using AutoMapper;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;

namespace DntSite.Web.Features.Projects.ModelsMappings;

public class AfterMapProjectFaqFormModel(IAppAntiXssService antiXssService)
    : IMappingAction<ProjectFaqFormModel, ProjectFaq>
{
    public void Process(ProjectFaqFormModel source, ProjectFaq destination, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.Description = antiXssService.GetSanitizedHtml(source.DescriptionText);
    }
}
