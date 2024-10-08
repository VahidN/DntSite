﻿using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.RoutingConstants;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Stats.Components;

[Authorize(Roles = CustomRoles.Admin)]
public partial class RecalculatePostsCount
{
    [InjectComponentScoped] internal IStatService StatService { set; get; } = null!;

    [InjectComponentScoped] internal ISiteReferrersService SiteReferrersService { set; get; } = null!;

    [Inject] internal IFullTextSearchService FullTextSearchService { set; get; } = null!;

    [SupplyParameterFromForm] internal RecalculatePostsCountAction RecalculateAction { set; get; }

    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private async Task OnValidSubmitAsync()
    {
        switch (RecalculateAction)
        {
            case RecalculatePostsCountAction.RecalculateForm:
                await StatService.RecalculateAllBlogPostsCommentsCountsAsync();
                await StatService.RecalculateTagsInUseCountsAsync<BlogPostTag, BlogPost>();
                await StatService.RecalculateAllUsersNumberOfPostsAndCommentsAsync();

                break;
            case RecalculatePostsCountAction.UpdateAllUsersRatings:
                await StatService.UpdateAllUsersRatingsAsync();

                break;
            case RecalculatePostsCountAction.UpdateFullTextIndex:
                FullTextSearchService.DeleteOldIndexFiles();

                break;

            case RecalculatePostsCountAction.DeleteAllSiteReferrers:
                await SiteReferrersService.DeleteAllAsync();

                break;
        }

        Alert.ShowAlert(AlertType.Success, title: "با تشکر!", message: "عملیات محاسبات مجدد، انجام شد.");
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        AddBreadCrumbs();
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..StatsBreadCrumbs.DefaultBreadCrumbs]);
}
