namespace DntSite.Web.Common.BlazorSsr.Models;

/// <summary>
///     The Alert Type
/// </summary>
public sealed class AlertType
{
    private AlertType(string value) => Value = value;

    public string Value { get; }

    /// <summary>
    ///     Success Alert
    /// </summary>
    public static AlertType Success => new("alert-success");

    /// <summary>
    ///     Info Alert
    /// </summary>
    public static AlertType Info => new("alert-primary");

    /// <summary>
    ///     Danger Alert
    /// </summary>
    public static AlertType Danger => new("alert-danger");

    /// <summary>
    ///     Warning Alert
    /// </summary>
    public static AlertType Warning => new("alert-warning");
}
