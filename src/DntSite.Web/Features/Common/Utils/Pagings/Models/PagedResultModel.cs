namespace DntSite.Web.Features.Common.Utils.Pagings.Models;

public class PagedResultModel<T>
    where T : class
{
    public IList<T> Data { get; set; } = new List<T>();

    public int TotalItems { get; set; }
}
