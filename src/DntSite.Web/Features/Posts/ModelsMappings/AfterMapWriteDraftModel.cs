using AutoMapper;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Models;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.Posts.ModelsMappings;

public class AfterMapWriteDraftModel(IAppAntiXssService antiXssService, ICurrentUserService currentUserService)
    : IMappingAction<WriteDraftModel, BlogPostDraft>
{
    public void Process(WriteDraftModel source, BlogPostDraft destination, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.Body = antiXssService.GetSanitizedHtml(source.ArticleBody);
        destination.UserId ??= currentUserService.GetCurrentUserId();

        if (!currentUserService.IsCurrentUserAdmin())
        {
            destination.DateTimeToShow = null;
        }
    }
}
