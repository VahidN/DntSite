namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom DntInputTag
/// </summary>
public partial class DntInputTag
{
    private string DataListId { get; } = Guid.NewGuid().ToString();

    [SupplyParameterFromForm] public string? NewTag { set; get; }

    /// <summary>
    ///     Additional user attributes.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    ///     The InputText's margin bottom. Its default value is `3`.
    /// </summary>
    [Parameter]
    public int InputRowMarginBottom { get; set; } = 3;

    /// <summary>
    ///     The InputText's margin bottom. Its default value is `3`.
    /// </summary>
    [Parameter]
    public int TagsRowMarginTop { get; set; } = 3;

    /// <summary>
    ///     Input field's icon from https://icons.getbootstrap.com/.
    /// </summary>
    [Parameter]
    public string FieldIcon { set; get; } = default!;

    [Parameter] public string FieldAddButtonIcon { set; get; } = default!;

    [Parameter] public string? FieldAddButtonTitle { set; get; }

    [Parameter] public string? FieldRemoveButtonTitle { set; get; }

    /// <summary>
    ///     The label name of the custom InputText
    /// </summary>
    [Parameter]
    public string LabelName { get; set; } = default!;

    /// <summary>
    ///     The InputText's column width. Its default value is `10`.
    /// </summary>
    [Parameter]
    public int InputTextColumnWidth { get; set; } = 10;

    /// <summary>
    ///     The label's column width of the custom InputText. Its default value is `2`.
    /// </summary>
    [Parameter]
    public int LabelColumnWidth { get; set; } = 2;

    [SupplyParameterFromForm] public HashSet<string>? EnteredTags { get; set; }

    /// <summary>
    ///     The tags list to display
    /// </summary>
    [Parameter]
    public IList<string>? InputTags { get; set; }

    /// <summary>
    ///     Fires when the tags list to display has been changed
    /// </summary>
    [Parameter]
    public EventCallback<IList<string>> InputTagsChanged { get; set; }

    [Parameter] public Expression<Func<IList<string>?>>? InputTagsExpression { get; set; }

    [Parameter] public IList<string>? AutoCompleteDataList { get; set; }

    [CascadingParameter] internal HttpContext HttpContext { set; get; } = null!;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (!HttpContext.IsPostRequest())
        {
            return;
        }

        if (!InputTagsChanged.HasDelegate)
        {
            return;
        }

        EnteredTags ??= new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (!string.IsNullOrWhiteSpace(NewTag))
        {
            EnteredTags.Add(NewTag);
        }

        _ = InputTagsChanged.InvokeAsync([.. EnteredTags]);
    }
}
