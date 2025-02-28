using DntSite.Web.Common.BlazorSsr.Extensions;
using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom QueryBuilder component
/// </summary>
public partial class DntQueryBuilder<TRecord>
    where TRecord : class
{
    private bool HasDefinedSearchRule => SearchRuleRows?.Count > 0;

    private static string FormName => $"QueryBuilder_{typeof(TRecord)}";

    private string GridifyFilter => SearchRuleRows.ToGridifyFilter();

    private Dictionary<DntQueryBuilderOperationKind, string> OperationKinds => new()
    {
        {
            DntQueryBuilderOperationKind.Is, IsLabel
        },
        {
            DntQueryBuilderOperationKind.Not, NotLabel
        }
    };

    private Dictionary<DntQueryBuilderOperationLogic, string> GroupLogics => new()
    {
        {
            DntQueryBuilderOperationLogic.And, AndLabel
        },
        {
            DntQueryBuilderOperationLogic.Or, OrLabel
        }
    };

    private List<DntQueryBuilderProperty<TRecord>> DefinedProperties { get; } = [];

    [Parameter] [EditorRequired] public required RenderFragment ChildContent { get; set; }

    [Parameter] [EditorRequired] public Func<string, Task>? OnSearch { get; set; }

    [Parameter] public string? PreviousFilter { set; get; }

    /// <summary>
    ///     Its default value is `Previous filter`.
    /// </summary>
    [Parameter]
    public string? PreviousFilterLabel { set; get; } = "Previous filter";

    /// <summary>
    ///     Its default value is `Start a new search`.
    /// </summary>
    [Parameter]
    public string? StartNewSearchLabel { set; get; } = "Start a new search";

    /// <summary>
    ///     Its default value is `/`.
    /// </summary>
    [Parameter]
    public string? StartNewSearchUrl { set; get; }

    /// <summary>
    ///     Its default value is `btn-sm btn btn-primary`.
    /// </summary>
    [Parameter]
    public string? StartNewSearchClass { set; get; } = "btn-sm btn btn-primary";

    /// <summary>
    ///     Its default value is `starts with`.
    /// </summary>
    [Parameter]
    public string StartsWithLabel { set; get; } = "starts with";

    /// <summary>
    ///     Its default value is `ends with`.
    /// </summary>
    [Parameter]
    public string EndsWithLabel { set; get; } = "ends with";

    /// <summary>
    ///     Its default value is `contains`.
    /// </summary>
    [Parameter]
    public string ContainsLabel { set; get; } = "contains";

    /// <summary>
    ///     Its default value is `equal`.
    /// </summary>
    [Parameter]
    public string EqualLabel { set; get; } = "equal";

    /// <summary>
    ///     Its default value is `less than`.
    /// </summary>
    [Parameter]
    public string LessThanLabel { set; get; } = "less than";

    /// <summary>
    ///     Its default value is `less than or equal`.
    /// </summary>
    [Parameter]
    public string LessThanOrEqualLabel { set; get; } = "less than or equal";

    /// <summary>
    ///     Its default value is `greater than`.
    /// </summary>
    [Parameter]
    public string GreaterThanLabel { set; get; } = "greater than";

    /// <summary>
    ///     Its default value is `greater than or equal`.
    /// </summary>
    [Parameter]
    public string GreaterThanOrEqualLabel { set; get; } = "greater than or equal";

    /// <summary>
    ///     Its default value is `and`.
    /// </summary>
    [Parameter]
    public string AndLabel { set; get; } = "and";

    /// <summary>
    ///     Its default value is `or`.
    /// </summary>
    [Parameter]
    public string OrLabel { set; get; } = "or";

    /// <summary>
    ///     Its default value is `is`.
    /// </summary>
    [Parameter]
    public string IsLabel { set; get; } = "is";

    /// <summary>
    ///     Its default value is `not`.
    /// </summary>
    [Parameter]
    public string NotLabel { set; get; } = "not";

    /// <summary>
    ///     Its default value is `Search`.
    /// </summary>
    [Parameter]
    public string SearchButtonLabel { set; get; } = "Search";

    /// <summary>
    ///     Its default value is `alert alert-info mt-3 mb-1`.
    /// </summary>
    [Parameter]
    public string GridifyFilterClass { set; get; } = "alert alert-info mt-3 mb-1";

    /// <summary>
    ///     Its default value is `badge bg-secondary fs-6`.
    /// </summary>
    [Parameter]
    public string RowPropertyTitleClass { set; get; } = "badge bg-secondary fs-6";

    /// <summary>
    ///     Its default value is `btn btn-primary btn-sm m-1`.
    /// </summary>
    [Parameter]
    public string SearchButtonClass { set; get; } = "btn btn-primary btn-sm m-1";

    /// <summary>
    ///     Its default value is `bi bi-search`.
    /// </summary>
    [Parameter]
    public string SearchButtonIconClass { set; get; } = DntBootstrapIcons.BiSearch;

    /// <summary>
    ///     Its default value is `btn btn-success btn-sm m-1`.
    /// </summary>
    [Parameter]
    public string AddRuleButtonClass { set; get; } = "btn btn-success btn-sm m-1";

    /// <summary>
    ///     Its default value is `d-flex justify-content-start mb-3`.
    /// </summary>
    [Parameter]
    public string ButtonsGroupDivClass { set; get; } = "d-flex justify-content-start mb-3";

    /// <summary>
    ///     Its default value is `bi bi-node-plus`.
    /// </summary>
    [Parameter]
    public string AddRuleButtonIconClass { set; get; } = DntBootstrapIcons.BiNodePlus;

    /// <summary>
    ///     Its default value is `Add a new search rule based on`.
    /// </summary>
    [Parameter]
    public string AddRuleButtonLabel { set; get; } = "Add a new search rule based on";

    /// <summary>
    ///     Its default value is `form-select w-auto`.
    /// </summary>
    [Parameter]
    public string FormSelectClass { set; get; } = "form-select w-auto";

    /// <summary>
    ///     Its default value is `form-control`.
    /// </summary>
    [Parameter]
    public string FormControlClass { set; get; } = "form-control";

    /// <summary>
    ///     Its default value is `btn btn-danger btn-sm`.
    /// </summary>
    [Parameter]
    public string DeleteRuleButtonClass { set; get; } = "btn btn-danger btn-sm";

    /// <summary>
    ///     Its default value is `bi bi-node-minus`.
    /// </summary>
    [Parameter]
    public string DeleteRuleButtonIconClass { set; get; } = DntBootstrapIcons.BiNodeMinus;

    /// <summary>
    ///     Its default value is `btn btn-danger btn-sm m-1`.
    /// </summary>
    [Parameter]
    public string DeleteAllRulesButtonClass { set; get; } = "btn btn-danger btn-sm m-1";

    /// <summary>
    ///     Its default value is `bi bi-node-minus`.
    /// </summary>
    [Parameter]
    public string DeleteAllRulesButtonIconClass { set; get; } = DntBootstrapIcons.BiNodeMinus;

    /// <summary>
    ///     Its default value is `Delete all rules`.
    /// </summary>
    [Parameter]
    public string DeleteAllRulesButtonLabel { set; get; } = "Delete all rules";

    /// <summary>
    ///     Defines the rest of the custom attributes.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    ///     The CSS class of the fieldset box.
    ///     Its default value is `border rounded p-2 shadow-sm m-3`.
    /// </summary>
    [Parameter]
    public string FieldSetClass { set; get; } = "border rounded p-2 shadow-sm mb-3";

    /// <summary>
    ///     The CSS class of the header inside the fieldset's legend.
    ///     Its default value is `badge bg-secondary`.
    /// </summary>
    [Parameter]
    public string LegendHeaderClass { set; get; } = "badge bg-secondary";

    /// <summary>
    ///     The CSS class of the fieldset's body.
    ///     Its default value is `card-body mb-2`.
    /// </summary>
    [Parameter]
    public string FieldSetBodyClass { set; get; } = "card-body mb-2";

    /// <summary>
    ///     Its default value is `row mb-2 g-1 d-flex flex-row align-items-center`.
    /// </summary>
    [Parameter]
    public string SearchRuleClass { set; get; } = "row mb-2 g-1 d-flex flex-row align-items-center";

    /// <summary>
    ///     The caption of the header inside the fieldset's legend.
    ///     Its default value is `Query Builder`.
    /// </summary>
    [Parameter]
    public string Header { set; get; } = "Query Builder";

    /// <summary>
    ///     Its default value is `true`.
    /// </summary>
    [Parameter]
    public bool ShowGridifyFilter { set; get; } = true;

    [SupplyParameterFromForm] public DntQueryBuilderAction QueryBuilderAction { set; get; }

    [SupplyParameterFromForm] public int AddRulePropertyIndex { set; get; }

    [SupplyParameterFromForm] public List<DntQueryBuilderSearchRule<TRecord>>? SearchRuleRows { set; get; }

    [SupplyParameterFromForm] public int? DeleteRowIndex { set; get; }

    /// <summary>
    ///     OnSearch Callback
    /// </summary>
    internal int AddProperty(DntQueryBuilderProperty<TRecord> property)
    {
        DefinedProperties.Add(property);

        return DefinedProperties.Count - 1;
    }

    private void DeleteAllRules() => SearchRuleRows?.Clear();

    private Task DoSearchAsync() => OnSearch is null ? Task.CompletedTask : OnSearch.Invoke(GridifyFilter);

    private Dictionary<DntQueryBuilderOperation, string> GetValidOperations(string? valueType)
    {
        var emptyResult = new Dictionary<DntQueryBuilderOperation, string>();

        if (string.IsNullOrWhiteSpace(valueType))
        {
            return emptyResult;
        }

        return valueType switch
        {
            "System.Guid" or "System.Boolean" => new Dictionary<DntQueryBuilderOperation, string>
            {
                {
                    DntQueryBuilderOperation.Equal, EqualLabel
                }
            },
            "System.Char" => emptyResult,
            "System.String" => new Dictionary<DntQueryBuilderOperation, string>
            {
                {
                    DntQueryBuilderOperation.Equal, EqualLabel
                },
                {
                    DntQueryBuilderOperation.LessThan, LessThanLabel
                },
                {
                    DntQueryBuilderOperation.LessThanOrEqual, LessThanOrEqualLabel
                },
                {
                    DntQueryBuilderOperation.GreaterThan, GreaterThanLabel
                },
                {
                    DntQueryBuilderOperation.GreaterThanOrEqual, GreaterThanOrEqualLabel
                },
                {
                    DntQueryBuilderOperation.StartsWith, StartsWithLabel
                },
                {
                    DntQueryBuilderOperation.EndsWith, EndsWithLabel
                },
                {
                    DntQueryBuilderOperation.Contains, ContainsLabel
                }
            },
            "System.SByte" or "System.Byte" or "System.Int16" or "System.UInt16" or "System.Int32" or "System.UInt32" or
                "System.Int64" or "System.UInt64" or "System.Decimal" or "System.Single" or "System.Double" or
                "System.DateTime" => new Dictionary<DntQueryBuilderOperation, string>
                {
                    {
                        DntQueryBuilderOperation.Equal, EqualLabel
                    },
                    {
                        DntQueryBuilderOperation.LessThan, LessThanLabel
                    },
                    {
                        DntQueryBuilderOperation.LessThanOrEqual, LessThanOrEqualLabel
                    },
                    {
                        DntQueryBuilderOperation.GreaterThan, GreaterThanLabel
                    },
                    {
                        DntQueryBuilderOperation.GreaterThanOrEqual, GreaterThanOrEqualLabel
                    }
                },
            "System.TimeSpan" => emptyResult,
            _ => emptyResult
        };
    }

    private Task OnValidSubmitAsync()
    {
        UpdateQueryBuilderSearchRules();

        if (ShouldRemoveRow())
        {
            if (DeleteRowIndex.Value >= 0 && DeleteRowIndex.Value < SearchRuleRows.Count)
            {
                SearchRuleRows.RemoveAt(DeleteRowIndex.Value);
            }

            return Task.CompletedTask;
        }

        switch (QueryBuilderAction)
        {
            case DntQueryBuilderAction.AddRule:
                AddRule();

                break;
            case DntQueryBuilderAction.DeleteAllRules:
                DeleteAllRules();

                break;
            case DntQueryBuilderAction.DoSearch:
                return DoSearchAsync();
        }

        return Task.CompletedTask;
    }

    [MemberNotNullWhen(returnValue: true, nameof(DeleteRowIndex))]
    [MemberNotNullWhen(returnValue: true, nameof(SearchRuleRows))]
    private bool ShouldRemoveRow() => DeleteRowIndex.HasValue && SearchRuleRows is not null;

    private void AddRule()
    {
        var property = DefinedProperties.Find(c => c.PropertyIndex == AddRulePropertyIndex);

        if (property is null)
        {
            return;
        }

        var rule = new DntQueryBuilderSearchRule<TRecord>
        {
            QueryBuilderProperty = property
        };

        SearchRuleRows ??= [];
        SearchRuleRows.Add(rule);

        rule.SearchRuleRowIndex = SearchRuleRows.Count - 1;
    }

    private void UpdateQueryBuilderSearchRules()
    {
        if (SearchRuleRows is null)
        {
            return;
        }

        for (var index = 0; index < SearchRuleRows.Count; index++)
        {
            var searchRule = SearchRuleRows[index];

            searchRule.SearchRuleRowIndex = index;

            searchRule.QueryBuilderProperty ??=
                DefinedProperties.Find(c => c.PropertyIndex == searchRule.RulePropertyIndex);
        }
    }
}
