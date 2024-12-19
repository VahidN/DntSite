namespace DntSite.Web.Features.Exports.Models;

public class ExportDocument
{
    public int Id { set; get; }

    public required string Title { set; get; }

    public required string Body { set; get; }

    public Guid DisplayId { set; get; }

    public required string PersianDate { set; get; }

    public required string Author { set; get; }

    public required string Url { set; get; }

    public required IList<string> Tags { set; get; }

    public required IList<ExportComment> Comments { set; get; }
}
