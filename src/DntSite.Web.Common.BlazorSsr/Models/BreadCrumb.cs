using System.Collections;
using System.Security.Claims;

namespace DntSite.Web.Common.BlazorSsr.Models;

/// <summary>
///     Represents the current BreadCrumb
/// </summary>
public class BreadCrumb : IEqualityComparer<BreadCrumb>, IEqualityComparer
{
    /// <summary>
    ///     A constant URL of the current item
    /// </summary>
    public string Url { set; get; } = default!;

    /// <summary>
    ///     Title of the current item
    /// </summary>
    public string Title { set; get; } = default!;

    /// <summary>
    ///     An optional glyph icon of the current item
    /// </summary>
    public string? GlyphIcon { set; get; }

    /// <summary>
    ///     Oder of the current item in the final list
    /// </summary>
    public int Order { set; get; }

    /// <summary>
    ///     Determines the active item in the list
    /// </summary>
    public bool IsActive { set; get; }

    /// <summary>
    ///     Is this item visible to anonymous/unauthenticated users?
    ///     Its default value is `true`.
    /// </summary>
    public bool AllowAnonymous { set; get; } = true;

    /// <summary>
    ///     Which comma separated roles are allowed to see this item?
    /// </summary>
    public string? AllowedRoles { set; get; }

    /// <summary>
    ///     Which user-claims are allowed to see this item?
    /// </summary>
    public IReadOnlyList<Claim>? AllowedClaims { set; get; }

    /// <summary>
    ///     FontWeightClass of the current item
    /// </summary>
    internal string? FontWeightClass { set; get; }

    internal string? ActiveClass { set; get; }

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

        if (x is BreadCrumb a && y is BreadCrumb b)
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

        if (obj is BreadCrumb x)
        {
            return GetHashCode(x);
        }

        throw new ArgumentException(message: "", nameof(obj));
    }

    public bool Equals(BreadCrumb? x, BreadCrumb? y)
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

        return string.Equals(x.Url, y.Url, StringComparison.Ordinal) &&
               string.Equals(x.Title, y.Title, StringComparison.Ordinal);
    }

    public int GetHashCode(BreadCrumb obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        return HashCode.Combine(obj.Url, obj.Title);
    }
}
