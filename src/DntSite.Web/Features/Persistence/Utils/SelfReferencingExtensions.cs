using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Persistence.Utils;

public static class SelfReferencingExtensions
{
    /// <summary>
    ///     Applying `AsNoTracking()` removes the auto-filling of `Children` and also `Include` methods only fetch one level of
    ///     hierarchy. So we need to create the final tree manually on the returned simple one-level in-memory list from EF.
    /// </summary>
    public static List<TEntity> ToSelfReferencingTree<TEntity>(this ICollection<TEntity>? originalList)
        where TEntity : BaseSelfReferencingEntity<TEntity>
    {
        var results = new List<TEntity>();

        if (originalList is null || originalList.Count == 0)
        {
            return results;
        }

        foreach (var rootItem in originalList.Where(x => !x.ReplyId.HasValue))
        {
            results.Add(rootItem);
            AppendChildren(originalList, rootItem);
        }

        return results;
    }

    private static void AppendChildren<TEntity>(ICollection<TEntity> originalList, TEntity parentItem)
        where TEntity : BaseSelfReferencingEntity<TEntity>
    {
        foreach (var kid in originalList.Where(x => x.ReplyId.HasValue && x.ReplyId.Value == parentItem.Id))
        {
            parentItem.Children ??= new List<TEntity>();
            parentItem.Children.Add(kid);
            AppendChildren(originalList, kid);
        }
    }
}
