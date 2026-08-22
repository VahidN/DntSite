namespace DntSite.Web.Features.Bookmarks.Models;

public class BookmarkDto
{
    public int BookmarkId { get; set; }

    public required string Title { get; set; }

    public required string Url { get; set; }

    public required string Type { get; set; }
}
