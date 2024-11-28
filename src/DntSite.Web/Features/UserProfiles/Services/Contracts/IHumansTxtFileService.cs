namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface IHumansTxtFileService : IScopedService
{
    /// <summary>
    ///     More info: http://humanstxt.org/Standard.html
    /// </summary>
    public Task CreateHumansTxtFileAsync();
}
