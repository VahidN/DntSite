using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.EfConfig;

public class BlogPostCommentReactionConfig : IEntityTypeConfiguration<BlogPostCommentReaction>
{
    public void Configure(EntityTypeBuilder<BlogPostCommentReaction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(reaction => reaction.Parent)
            .WithMany(entity => entity.Reactions)
            .HasForeignKey(reaction => reaction.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BlogPostCommentReactions)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        builder.HasOne(entity => entity.ForUser)
            .WithMany(user => user.BlogPostCommentReactionsForUsers)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);
    }
}
