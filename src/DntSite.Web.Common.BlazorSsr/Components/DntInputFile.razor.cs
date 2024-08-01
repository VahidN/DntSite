using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntInputFile
{
    private BlazorHtmlField<IFormFileCollection?> ValueField => new(FilesExpression);

    /// <summary>
    ///     Additional user attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    ///     The InputFile's margin bottom. Its default value is `3`.
    /// </summary>
    [Parameter]
    public int InputRowMarginBottom { get; set; } = 3;

    /// <summary>
    ///     The InputFiles column width. Its default value is `9`.
    /// </summary>
    [Parameter]
    public int InputFileColumnWidth { get; set; } = 9;

    /// <summary>
    ///     The label's column width of the custom InputFile. Its default value is `3`.
    /// </summary>
    [Parameter]
    public int LabelColumnWidth { get; set; } = 3;

    /// <summary>
    ///     The label name of the custom InputText
    /// </summary>
    [Parameter]
    public string LabelName { get; set; } = default!;

    /// <summary>
    ///     The optional label name of the custom InputText
    /// </summary>
    [Parameter]
    public string? OptionalLabel { get; set; }

    /// <summary>
    ///     Its default value is `Select file(s)`.
    /// </summary>
    [Parameter]
    public string InputFileLabel { set; get; } = "انتخاب فایل";

    /// <summary>
    ///     Its default value is `btn btn-secondary`.
    /// </summary>
    [Parameter]
    public string InputFileButtonClass { set; get; } = "btn btn-secondary";

    /// <summary>
    ///     Its default value is `bi-files` from https://icons.getbootstrap.com/.
    /// </summary>
    [Parameter]
    public string InputFileButtonIconClass { set; get; } = DntBootstrapIcons.BiFiles;

    [Parameter] public Expression<Func<IFormFileCollection?>> FilesExpression { get; set; } = default!;

    [Parameter] public IFormFileCollection? Files { get; set; }

    [Parameter] public EventCallback<IFormFileCollection?> FilesChanged { get; set; }

    [Parameter] public bool AllowSelectingMultipleFiles { set; get; }

    [Parameter] public string? AcceptedFileFormats { get; set; } = ".zip,.rar,.7z";
}
