using DntSite.Web.Features.Backlogs.Entities;

namespace DntSite.Web.Features.Backlogs.EfConfig;

public class BacklogCommentVisitorConfig : IEntityTypeConfiguration<BacklogCommentVisitor>
{
    public void Configure(EntityTypeBuilder<BacklogCommentVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(commentVisitor => commentVisitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(commentVisitor => commentVisitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BacklogCommentVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
