namespace DntSite.Web.Common.BlazorSsr.Models;

/// <summary>
///     Allows the browser to choose a correct autocomplete for the input fields
/// </summary>
public sealed class AutoCompleteType
{
    private AutoCompleteType(string value) => Value = value;

    public string Value { get; }

    /// <summary>
    ///     For “enter new password” or “confirm new password” fields
    /// </summary>
    public static AutoCompleteType NewPassword => new("new-password");

    /// <summary>
    ///     For a login page
    /// </summary>
    public static AutoCompleteType CurrentPassword => new("current-password");

    /// <summary>
    ///     For a login page
    /// </summary>
    public static AutoCompleteType UserName => new("username");

    /// <summary>
    ///     Specifies that autocomplete is off (disabled)
    /// </summary>
    public static AutoCompleteType Off => new("off");

    /// <summary>
    ///     Specifies that autocomplete is on (enabled)
    /// </summary>
    public static AutoCompleteType On => new("on");
}
