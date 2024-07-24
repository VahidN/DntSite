using DntSite.Web.Features.Stats.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface IEfYearStatService : IScopedService
{
    Task<IList<AnnualStatisticsInfo>> GetAnnualStatisticsAsync(string persianYear);

    Task<AnnualStatisticsInfo> GetRegisteredUsersAsync(string persianYear);

    Task<AnnualStatisticsInfo> GetSurveysAsync(string persianYear);

    Task<AnnualStatisticsInfo> GetLinksAsync(string persianYear);

    Task<AnnualStatisticsInfo> GetProjectsAsync(string persianYear);

    Task<AnnualStatisticsInfo> GetYearCoursesAsync(string persianYear);

    Task<AnnualStatisticsInfo> GetYearArticlesAsync(string persianYear);

    Task<IList<SelectListItem>> PersianYearsOfWritingAsync();
}
