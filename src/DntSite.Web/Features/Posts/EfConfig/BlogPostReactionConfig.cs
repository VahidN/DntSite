using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.EfConfig;

public class BlogPostReactionConfig : IEntityTypeConfiguration<BlogPostReaction>
{
    public void Configure(EntityTypeBuilder<BlogPostReaction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(reaction => reaction.Parent)
            .WithMany(entity => entity.Reactions)
            .HasForeignKey(reaction => reaction.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BlogPostReactions)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        builder.HasOne(entity => entity.ForUser)
            .WithMany(user => user.BlogPostReactionsForUsers)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);
    }
}
