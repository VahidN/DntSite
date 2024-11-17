using Microsoft.AspNetCore.Mvc.Rendering;

namespace DntSite.Web.Features.Stats.Models;

public class AnnualStatisticsModel
{
    public IList<AnnualStatisticsInfo> AnnualStatisticsInfo { set; get; } = [];

    public int PersianYear { set; get; }

    public IList<SelectListItem> PersianYears { set; get; } = [];
}
