using AutoMapper;
using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Advertisements.Models;

namespace DntSite.Web.Features.Advertisements.ModelsMappings;

public class AfterMapWriteAdvertisementModel(
    IAntiXssService antiXssService) : IMappingAction<WriteAdvertisementModel, Advertisement>
{
    public void Process(WriteAdvertisementModel source, Advertisement destination, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.Body = antiXssService.GetSanitizedHtml(source.Body);
    }
}
