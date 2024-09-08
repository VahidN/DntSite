using AutoMapper;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;

namespace DntSite.Web.Features.Projects.ModelsMappings;

public class AfterMapProjectModel(IAppAntiXssService antiXssService) : IMappingAction<ProjectModel, Project>
{
    public void Process(ProjectModel source, Project destination, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.Description = antiXssService.GetSanitizedHtml(source.DescriptionText);
        destination.RequiredDependencies = antiXssService.GetSanitizedHtml(source.RequiredDependenciesText);
        destination.RelatedArticles = antiXssService.GetSanitizedHtml(source.RelatedArticlesText);
        destination.DevelopersDescription = antiXssService.GetSanitizedHtml(source.DevelopersDescriptionText);
        destination.License = antiXssService.GetSanitizedHtml(source.LicenseText);
    }
}
