using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Caching.Memory;

namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntCacheComponent<TComponent>
    where TComponent : IComponent
{
    private const string CacheKeyPrefix = $"__{nameof(DntCacheComponent<TComponent>)}__";
    private string? _cachedContent;

    [Inject] internal HtmlRenderer HtmlRenderer { set; get; } = null!;

    [Inject] internal ICacheService CacheService { get; set; } = null!;

    /// <summary>
    ///     Parameters for the component.
    /// </summary>
    [Parameter]
    public IDictionary<string, object?>? Parameters { set; get; }

    /// <summary>
    ///     Gets or sets the exact <see cref="DateTimeOffset" /> the cache entry should be evicted.
    /// </summary>
    [Parameter]
    public DateTimeOffset? ExpiresOn { get; set; }

    /// <summary>
    ///     Gets or sets the duration, from the time the cache entry was added, when it should be evicted.
    /// </summary>
    [Parameter]
    public TimeSpan? ExpiresAfter { get; set; }

    /// <summary>
    ///     Gets or sets the duration from last access that the cache entry should be evicted.
    /// </summary>
    [Parameter]
    public TimeSpan? ExpiresSliding { get; set; }

    /// <summary>
    ///     Gets or sets the <see cref="CacheItemPriority" /> policy for the cache entry.
    /// </summary>
    [Parameter]
    public CacheItemPriority? Priority { get; set; }

    /// <summary>
    ///     Gets or sets the key of the cache entry.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public required string CacheKey { get; set; }

    private string CacheEntryKey => $"{CacheKeyPrefix}{CacheKey}";

    protected override Task OnInitializedAsync() => ProcessAsync();

    public void InvalidateCache() => CacheService.Remove(CacheEntryKey);

    private async Task ProcessAsync()
    {
        if (!CacheService.TryGetValue(CacheEntryKey, out _cachedContent))
        {
            _cachedContent = await HtmlRenderer.Dispatcher.InvokeAsync(async () =>
            {
                var output = await HtmlRenderer.RenderComponentAsync<TComponent>(Parameters is null
                    ? ParameterView.Empty
                    : ParameterView.FromDictionary(Parameters));

                return output.ToHtmlString();
            });

            CacheService.Add(CacheEntryKey, _cachedContent,
                ExpiresOn.GetMemoryCacheEntryOptions(ExpiresAfter, ExpiresSliding, Priority));
        }
    }
}
