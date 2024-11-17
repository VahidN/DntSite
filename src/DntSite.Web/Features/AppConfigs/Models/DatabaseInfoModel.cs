namespace DntSite.Web.Features.AppConfigs.Models;

public class DatabaseInfoModel
{
    public long DatabaseSizeInBytes { set; get; }

    public long DatabaseFreeSpaceSizeInBytes { set; get; }

    public IList<string> CompileOptions { set; get; } = [];

    public required string DatabaseVersion { set; get; }

    public IList<SQLitePragma> Pragmas { set; get; } = [];

    public IList<SQLiteTable> Tables { set; get; } = [];
}
