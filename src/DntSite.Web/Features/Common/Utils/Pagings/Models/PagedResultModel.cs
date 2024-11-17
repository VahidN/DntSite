namespace DntSite.Web.Features.Common.Utils.Pagings.Models;

public class PagedResultModel<T>
    where T : class
{
    public IList<T> Data { get; set; } = [];

    public int TotalItems { get; set; }
}
