namespace DntSite.Web.Common.BlazorSsr.Models;

public sealed class TextColor
{
    private TextColor(string value) => Value = value;

    public string Value { get; }

    public static TextColor Primary => new("text-primary");

    public static TextColor Secondary => new("text-secondary");

    public static TextColor Success => new("text-success");

    public static TextColor Danger => new("text-danger");

    public static TextColor Warning => new("text-warning");

    public static TextColor Info => new("text-info");

    public static TextColor Dark => new("text-dark");
}
