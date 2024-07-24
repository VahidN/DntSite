using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.Services.Contracts;

public interface IBlogCommentsEmailsService : IScopedService
{
    Task PostReplySendEmailToAdminsAsync(BlogPostComment data);

    Task PostReplySendEmailToWritersAsync(BlogPostComment comment);

    Task PostReplySendEmailToPersonAsync(BlogPostComment comment);

    Task ConvertedToReplySendEmailAsync(int postId, string title, int userIdValue);
}
