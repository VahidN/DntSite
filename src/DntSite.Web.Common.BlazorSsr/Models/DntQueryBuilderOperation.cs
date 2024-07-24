namespace DntSite.Web.Common.BlazorSsr.Models;

/// <summary>
///     Defines the available operations
/// </summary>
public enum DntQueryBuilderOperation
{
    /// <summary>
    ///     Its default value is `starts with`.
    /// </summary>
    StartsWith,

    /// <summary>
    ///     Its default value is `ends with`.
    /// </summary>
    EndsWith,

    /// <summary>
    ///     Its default value is `contains`.
    /// </summary>
    Contains,

    /// <summary>
    ///     Its default value is `equal`.
    /// </summary>
    Equal,

    /// <summary>
    ///     Its default value is `less than`.
    /// </summary>
    LessThan,

    /// <summary>
    ///     Its default value is `less than or equal`.
    /// </summary>
    LessThanOrEqual,

    /// <summary>
    ///     Its default value is `greater than`.
    /// </summary>
    GreaterThan,

    /// <summary>
    ///     Its default value is `greater than or equal`.
    /// </summary>
    GreaterThanOrEqual
}
