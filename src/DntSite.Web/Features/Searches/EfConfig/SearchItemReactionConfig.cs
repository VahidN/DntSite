using DntSite.Web.Features.Searches.Entities;

namespace DntSite.Web.Features.Searches.EfConfig;

public class SearchItemReactionConfig : IEntityTypeConfiguration<SearchItemReaction>
{
    public void Configure(EntityTypeBuilder<SearchItemReaction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(reaction => reaction.Parent)
            .WithMany(entity => entity.Reactions)
            .HasForeignKey(reaction => reaction.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SearchItemReactions)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        builder.HasOne(entity => entity.ForUser)
            .WithMany(user => user.SearchItemReactionsForUsers)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);
    }
}
