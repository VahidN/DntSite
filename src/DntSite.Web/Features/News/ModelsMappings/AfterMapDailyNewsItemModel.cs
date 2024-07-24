using AutoMapper;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.Models;

namespace DntSite.Web.Features.News.ModelsMappings;

public class AfterMapDailyNewsItemModel(
    IAntiXssService antiXssService,
    IUrlNormalizationService urlNormalizationService,
    IPasswordHasherService passwordHasherService) : IMappingAction<DailyNewsItemModel, DailyNewsItem>
{
    public void Process(DailyNewsItemModel source, DailyNewsItem destination, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.BriefDescription = antiXssService.GetSanitizedHtml(source.DescriptionText);

        destination.UrlHash =
            passwordHasherService.GetSha1Hash(urlNormalizationService.NormalizeUrl(source.Url.Trim()));
    }
}
