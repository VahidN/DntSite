using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.Components;

public partial class SurveyResults
{
    [Parameter] [EditorRequired] public Survey? Survey { set; get; }

    private double GetItemPercent(SurveyItem? item)
    {
        if (Survey is null || item is null || Survey.TotalRaters == 0)
        {
            return 0;
        }

        return (double)(item.TotalSurveys * 100) / Survey.TotalRaters;
    }

    private string GetProgressBarType(double percent)
        => percent switch
        {
            < 30 => "bg-danger",
            >= 30 and < 50 => "bg-warning",
            >= 50 and < 80 => "bg-info",
            _ => "bg-success"
        };
}
