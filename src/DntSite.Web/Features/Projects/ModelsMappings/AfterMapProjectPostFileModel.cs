using AutoMapper;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;

namespace DntSite.Web.Features.Projects.ModelsMappings;

public class AfterMapProjectPostFileModel(IAntiXssService antiXssService)
    : IMappingAction<ProjectPostFileModel, ProjectRelease>
{
    public void Process(ProjectPostFileModel source, ProjectRelease destination, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.FileDescription = antiXssService.GetSanitizedHtml(source.Description);
    }
}
