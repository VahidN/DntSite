using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.Services.Contracts;

public interface IBlogCommentsEmailsService : IScopedService
{
    public Task PostReplySendEmailToAdminsAsync(BlogPostComment data);

    public Task PostReplySendEmailToWritersAsync(BlogPostComment comment);

    public Task PostReplySendEmailToPersonAsync(BlogPostComment comment);

    public Task ConvertedToReplySendEmailAsync(int postId, string title, int userIdValue);
}
