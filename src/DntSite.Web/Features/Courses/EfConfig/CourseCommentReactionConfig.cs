using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.EfConfig;

public class CourseCommentReactionConfig : IEntityTypeConfiguration<CourseCommentReaction>
{
    public void Configure(EntityTypeBuilder<CourseCommentReaction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(reaction => reaction.Parent)
            .WithMany(entity => entity.Reactions)
            .HasForeignKey(reaction => reaction.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.CourseCommentReactions)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        builder.HasOne(entity => entity.ForUser)
            .WithMany(user => user.CourseCommentReactionsForUsers)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);
    }
}
