namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntToPersianNumbers
{
    private string? _formattedNumber;

    [EditorRequired] [Parameter] public object? Value { set; get; }

    /// <summary>
    ///     Method invoked when the component has received parameters from its parent.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        _formattedNumber = FormatValueAsString(Value);
    }

    private static string? FormatValueAsString(object? value)
    {
        var formattedValue = string.Format(CultureInfo.InvariantCulture, "{0:n0}", value);

        return formattedValue.ToPersianNumbers();
    }
}
