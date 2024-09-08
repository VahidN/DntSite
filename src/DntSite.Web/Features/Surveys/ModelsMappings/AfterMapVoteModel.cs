using AutoMapper;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.Models;

namespace DntSite.Web.Features.Surveys.ModelsMappings;

public class AfterMapVoteModel(IAppAntiXssService antiXssService) : IMappingAction<VoteModel, Survey>
{
    public void Process(VoteModel source, Survey destination, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.Description = antiXssService.GetSanitizedHtml(source.Description);
    }
}
