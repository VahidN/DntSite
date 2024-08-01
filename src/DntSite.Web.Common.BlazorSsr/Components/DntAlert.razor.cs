namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntAlert
{
    private AlertType _alertType = AlertType.Info;
    private bool _isModalVisible;
    private string? _message;
    private string? _title;

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    [Parameter] public RenderFragment? ChildContent { get; set; }

    private string MainDivId { get; } = Guid.NewGuid().ToString(format: "N");

    private int Hash => HashCode.Combine(_alertType, _title, _message);

    public void HideAlert()
    {
        _isModalVisible = false;

        StateHasChanged();
    }

    public void ShowAlert(AlertType alertType, string? title, string? message)
    {
        _alertType = alertType;
        _title = title;
        _message = message;
        _isModalVisible = true;

        StateHasChanged();
    }
}
