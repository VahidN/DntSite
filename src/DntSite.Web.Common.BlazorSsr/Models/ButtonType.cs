using System.Collections;

namespace DntSite.Web.Common.BlazorSsr.Models;

public sealed class ButtonType : IEqualityComparer<ButtonType>, IEqualityComparer
{
    private ButtonType(string value) => Value = value;

    public string Value { get; }

    public static ButtonType Button => new(value: "button");

    public static ButtonType Submit => new(value: "submit");

    public new bool Equals(object? x, object? y)
    {
        if (x == y)
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        if (x is ButtonType a && y is ButtonType b)
        {
            return Equals(a, b);
        }

        throw new ArgumentException(message: "", nameof(x));
    }

    public int GetHashCode(object? obj)
    {
        if (obj is null)
        {
            return 0;
        }

        if (obj is ButtonType x)
        {
            return GetHashCode(x);
        }

        throw new ArgumentException(message: "", nameof(obj));
    }

    public bool Equals(ButtonType? x, ButtonType? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null)
        {
            return false;
        }

        if (y is null)
        {
            return false;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        return string.Equals(x.Value, y.Value, StringComparison.Ordinal);
    }

    public int GetHashCode(ButtonType obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        return HashCode.Combine(obj.Value);
    }
}
