namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntIconLink
{
    /// <summary>
    ///     Additional user attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    ///     Its default value is true
    /// </summary>
    [Parameter]
    public bool IsVisible { set; get; } = true;

    [Parameter] [EditorRequired] public required string Url { set; get; }

    [Parameter] [EditorRequired] public required string Width { set; get; }

    [Parameter] [EditorRequired] public required string Height { set; get; }

    [EditorRequired] [Parameter] public required string GlyphIcon { set; get; }

    [Parameter] [EditorRequired] public required string AltName { set; get; }

    [Parameter] [EditorRequired] public bool IsExternal { set; get; }

    /// <summary>
    ///     Its default value is `2`
    /// </summary>
    [Parameter]
    public int MarginLeft { set; get; } = 2;
}
