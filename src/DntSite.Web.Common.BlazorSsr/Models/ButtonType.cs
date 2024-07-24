namespace DntSite.Web.Common.BlazorSsr.Models;

public sealed class ButtonType
{
    private ButtonType(string value) => Value = value;

    public string Value { get; }

    public static ButtonType Button => new("button");

    public static ButtonType Submit => new("submit");
}
