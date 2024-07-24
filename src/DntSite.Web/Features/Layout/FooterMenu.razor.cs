using DntSite.Web.Features.Posts.Services.Contracts;

namespace DntSite.Web.Features.Layout;

public partial class FooterMenu
{
    private string? _copyrightMessage;

    [InjectComponentScoped] internal IBlogPostsService BlogPostsService { set; get; } = null!;

    [Parameter] [EditorRequired] public required string SiteName { set; get; }

    protected override Task OnInitializedAsync() => SetCopyrightMessageAsync();

    private async Task SetCopyrightMessageAsync()
    {
        var firstPost = await BlogPostsService.GetFirstBlogPostAsync();
        var currentPersianYear = DateTime.UtcNow.GetPersianYear();

        _copyrightMessage = firstPost is null
            ? Invariant($"© {SiteName}, {currentPersianYear}")
            : Invariant($"© {SiteName}, {firstPost.Audit.CreatedAt.GetPersianYear()}-{currentPersianYear}");
    }
}
