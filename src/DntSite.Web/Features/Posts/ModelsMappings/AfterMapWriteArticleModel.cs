using AutoMapper;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Models;

namespace DntSite.Web.Features.Posts.ModelsMappings;

public class AfterMapWriteArticleModel(IAppAntiXssService antiXssService) : IMappingAction<WriteArticleModel, BlogPost>
{
    public void Process(WriteArticleModel source, BlogPost destination, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.Body = antiXssService.GetSanitizedHtml(source.ArticleBody);
    }
}
