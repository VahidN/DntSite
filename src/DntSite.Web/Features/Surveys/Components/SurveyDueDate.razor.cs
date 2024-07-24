using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.Components;

public partial class SurveyDueDate
{
    [Parameter] [EditorRequired] public Survey? Survey { set; get; }
}
