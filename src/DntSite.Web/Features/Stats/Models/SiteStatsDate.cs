using System.ComponentModel;

namespace DntSite.Web.Features.Stats.Models;

public enum SiteStatsDate
{
    [Description("امروز")] Today,
    [Description("دیروز")] Yesterday,
    [Description("ماه جاری")] ThisMonth,
    [Description("سال جاری")] ThisYear,
    [Description("ماه قبل")] LastMonth,
    [Description("سال قبل")] LastYear,
    [Description("از ابتدا")] FromTheBeginning
}
