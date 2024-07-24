namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntSpinner
{
    [Parameter] [EditorRequired] public required TextColor Color { get; set; }

    [Parameter] [EditorRequired] public required SpinnerSize Size { get; set; }

    [Parameter] public int Margin { get; set; } = 5;

    [Parameter] public string LoadingText { get; set; } = string.Empty;
}
