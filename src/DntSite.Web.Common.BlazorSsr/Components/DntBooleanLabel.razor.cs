using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntBooleanLabel
{
    [Parameter] [EditorRequired] public bool Value { set; get; }

    [Parameter] public string? TrueValueLabel { set; get; } = "بله";

    [Parameter] public string? FalseValueLabel { set; get; } = "خیر";

    [Parameter] public string? TrueValueIcon { set; get; } = DntBootstrapIcons.BiCheckCircle;

    [Parameter] public string? FalseValueIcon { set; get; } = DntBootstrapIcons.BiXCircle;
}
