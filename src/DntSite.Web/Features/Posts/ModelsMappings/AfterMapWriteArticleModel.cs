using AutoMapper;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Models;

namespace DntSite.Web.Features.Posts.ModelsMappings;

public class AfterMapWriteArticleModel(IAntiXssService antiXssService) : IMappingAction<WriteArticleModel, BlogPost>
{
    public void Process(WriteArticleModel source, BlogPost destination, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.Body = antiXssService.GetSanitizedHtml(source.ArticleBody);
    }
}
