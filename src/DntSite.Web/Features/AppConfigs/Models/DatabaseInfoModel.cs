namespace DntSite.Web.Features.AppConfigs.Models;

public class DatabaseInfoModel
{
    public long DatabaseSizeInBytes { set; get; }

    public long DatabaseFreeSpaceSizeInBytes { set; get; }

    public IList<string> CompileOptions { set; get; } = new List<string>();

    public required string DatabaseVersion { set; get; }

    public IList<SQLitePragma> Pragmas { set; get; } = new List<SQLitePragma>();
}
