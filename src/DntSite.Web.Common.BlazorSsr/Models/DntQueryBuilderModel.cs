namespace DntSite.Web.Common.BlazorSsr.Models;

public class DntQueryBuilderModel
{
    public string? SortBy { get; set; }

    public bool IsAscending { get; set; } = true;

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 15;

    public string? GridifyFilter { get; set; }
}
