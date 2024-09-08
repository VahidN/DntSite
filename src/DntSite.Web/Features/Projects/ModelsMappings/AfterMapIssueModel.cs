using AutoMapper;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;

namespace DntSite.Web.Features.Projects.ModelsMappings;

public class AfterMapIssueModel(IAppAntiXssService antiXssService) : IMappingAction<IssueModel, ProjectIssue>
{
    public void Process(IssueModel source, ProjectIssue destination, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.Description = antiXssService.GetSanitizedHtml(source.Description);
    }
}
