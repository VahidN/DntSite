using DntSite.Web.Features.Stats.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface IEfYearStatService : IScopedService
{
    public Task<IList<AnnualStatisticsInfo>> GetAnnualStatisticsAsync(string persianYear);

    public Task<AnnualStatisticsInfo> GetRegisteredUsersAsync(string persianYear);

    public Task<AnnualStatisticsInfo> GetSurveysAsync(string persianYear);

    public Task<AnnualStatisticsInfo> GetLinksAsync(string persianYear);

    public Task<AnnualStatisticsInfo> GetProjectsAsync(string persianYear);

    public Task<AnnualStatisticsInfo> GetYearCoursesAsync(string persianYear);

    public Task<AnnualStatisticsInfo> GetYearArticlesAsync(string persianYear);

    public Task<IList<SelectListItem>> PersianYearsOfWritingAsync();
}
