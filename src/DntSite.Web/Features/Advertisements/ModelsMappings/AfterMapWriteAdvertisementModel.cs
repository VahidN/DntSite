using AutoMapper;
using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Advertisements.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;

namespace DntSite.Web.Features.Advertisements.ModelsMappings;

public class AfterMapWriteAdvertisementModel(IAppAntiXssService antiXssService)
    : IMappingAction<WriteAdvertisementModel, Advertisement>
{
    public void Process(WriteAdvertisementModel source, Advertisement destination, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.Body = antiXssService.GetSanitizedHtml(source.Body);
    }
}
