namespace DntSite.Web.Features.UserFiles.RoutingConstants;

public static class UserFilesRoutingConstants
{
    public const string FilesManagerBase = "/files-manager";
    public const string FilesManagerType = $"{FilesManagerBase}/{{Type}}";
    public const string FilesManagerTypePageCurrentPage = $"{FilesManagerType}/page/{{CurrentPage:int?}}";

    public const string FilesManagerFilesUrl = $"{FilesManagerBase}/users-files";
    public const string FilesManagerImagesUrl = $"{FilesManagerBase}/users-images";
}
