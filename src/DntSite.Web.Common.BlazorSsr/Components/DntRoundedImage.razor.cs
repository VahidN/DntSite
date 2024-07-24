namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntRoundedImage
{
    [Parameter] [EditorRequired] public required string ImageUrl { set; get; }

    [Parameter] [EditorRequired] public required string AltName { set; get; }

    [Parameter] [EditorRequired] public required string Width { set; get; }

    [Parameter] [EditorRequired] public required string Height { set; get; }

    /// <summary>
    ///     Its default value is `1`
    /// </summary>
    [Parameter]
    public int MarginLeft { set; get; } = 1;
}
