using AutoMapper;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Backlogs.Models;

namespace DntSite.Web.Features.Backlogs.ModelsMappings;

public class AfterMapBacklogModel(IAppAntiXssService antiXssService) : IMappingAction<BacklogModel, Backlog>
{
    public void Process(BacklogModel source, Backlog destination, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.Description = antiXssService.GetSanitizedHtml(source.Description);
    }
}
