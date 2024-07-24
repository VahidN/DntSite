namespace DntSite.Web.Features.UserFiles.Models;

public class FileModel
{
    public required string Name { set; get; }

    public DateTime LastWriteTime { set; get; }

    public long Size { set; get; }

    public required string Icon { set; get; }
}
