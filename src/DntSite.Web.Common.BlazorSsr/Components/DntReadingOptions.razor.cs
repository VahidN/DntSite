namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntReadingOptions
{
    [Parameter] [EditorRequired] public int ReadingTimeMinutes { set; get; }
}
