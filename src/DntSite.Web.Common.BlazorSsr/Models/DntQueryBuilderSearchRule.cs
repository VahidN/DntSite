using System.Runtime.Serialization;
using DntSite.Web.Common.BlazorSsr.Components;

namespace DntSite.Web.Common.BlazorSsr.Models;

/// <summary>
///     Defines a search rule
/// </summary>
public class DntQueryBuilderSearchRule<TRecord>
    where TRecord : class
{
    public int RulePropertyIndex { set; get; }

    [IgnoreDataMember] public DntQueryBuilderProperty<TRecord>? QueryBuilderProperty { set; get; }

    [IgnoreDataMember] public int SearchRuleRowIndex { set; get; }

    /// <summary>
    ///     Defines `is` or `is not`
    /// </summary>
    public DntQueryBuilderOperationKind? OperationKind { set; get; }

    /// <summary>
    ///     Defines the operation type such as StartsWith
    /// </summary>
    public DntQueryBuilderOperation? Operation { set; get; }

    /// <summary>
    ///     Defines the assigned data value
    /// </summary>
    public string? Value { set; get; }

    /// <summary>
    ///     Defines the logic, which can be `and` or `or`.
    /// </summary>
    public DntQueryBuilderOperationLogic NextOperationLogic { set; get; }

    public string RulePropertyIndexName => string.Create(CultureInfo.InvariantCulture,
        $"{nameof(DntQueryBuilder<TRecord>.SearchRuleRows)}[{SearchRuleRowIndex}].{nameof(RulePropertyIndex)}");

    public string RuleOperationKindName => string.Create(CultureInfo.InvariantCulture,
        $"{nameof(DntQueryBuilder<TRecord>.SearchRuleRows)}[{SearchRuleRowIndex}].{nameof(OperationKind)}");

    public string RuleOperationName => string.Create(CultureInfo.InvariantCulture,
        $"{nameof(DntQueryBuilder<TRecord>.SearchRuleRows)}[{SearchRuleRowIndex}].{nameof(Operation)}");

    public string RuleValueName => string.Create(CultureInfo.InvariantCulture,
        $"{nameof(DntQueryBuilder<TRecord>.SearchRuleRows)}[{SearchRuleRowIndex}].{nameof(Value)}");

    public string RuleNextOperationLogicName => string.Create(CultureInfo.InvariantCulture,
        $"{nameof(DntQueryBuilder<TRecord>.SearchRuleRows)}[{SearchRuleRowIndex}].{nameof(NextOperationLogic)}");
}
