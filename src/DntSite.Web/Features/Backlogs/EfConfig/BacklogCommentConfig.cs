using DntSite.Web.Features.Backlogs.Entities;

namespace DntSite.Web.Features.Backlogs.EfConfig;

public class BacklogCommentConfig : IEntityTypeConfiguration<BacklogComment>
{
    public void Configure(EntityTypeBuilder<BacklogComment> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(comment => comment.Body).IsRequired();

        builder.HasOne(backlogComment => backlogComment.Parent)
            .WithMany(backlog => backlog.Comments)
            .HasForeignKey(backlogComment => backlogComment.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BacklogComments)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        // Self Referencing Entity
        builder.HasIndex(comment => comment.ReplyId);

        // By convention, a property whose CLR type can contain null will be configured as `optional`.
        builder.HasOne(comment => comment.Reply)
            .WithMany(advertisementComment => advertisementComment.Children)
            .HasForeignKey(comment => comment.ReplyId)
            .IsRequired(false);
    }
}
