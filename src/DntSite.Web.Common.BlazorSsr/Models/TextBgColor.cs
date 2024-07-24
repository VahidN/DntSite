namespace DntSite.Web.Common.BlazorSsr.Models;

public sealed class TextBgColor
{
    private TextBgColor(string value) => Value = value;

    public string Value { get; }

    public static TextBgColor Primary => new("text-bg-primary");

    public static TextBgColor Secondary => new("text-bg-secondary");

    public static TextBgColor Success => new("text-bg-success");

    public static TextBgColor Danger => new("text-bg-danger");

    public static TextBgColor Warning => new("text-bg-warning");

    public static TextBgColor Info => new("text-bg-info");

    public static TextBgColor Dark => new("text-bg-dark");

    public static TextBgColor Light => new("text-bg-light");
}
