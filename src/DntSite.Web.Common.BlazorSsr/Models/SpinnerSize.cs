namespace DntSite.Web.Common.BlazorSsr.Models;

public sealed class SpinnerSize
{
    private SpinnerSize(string value) => Value = value;

    public string Value { get; }

    public static SpinnerSize Small => new("small-spinner");

    public static SpinnerSize Normal => new("normal-spinner");

    public static SpinnerSize Medium => new("medium-spinner");

    public static SpinnerSize Large => new("large-spinner");
}
