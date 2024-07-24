using DntSite.Web.Features.Backlogs.Entities;

namespace DntSite.Web.Features.Backlogs.EfConfig;

public class BacklogConfig : IEntityTypeConfiguration<Backlog>
{
    public void Configure(EntityTypeBuilder<Backlog> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(backlog => backlog.Title).HasMaxLength(450).IsRequired();
        builder.Property(backlog => backlog.Description).IsRequired();

        builder.HasOne(backlog => backlog.DoneByUser)
            .WithMany(user => user.DoneBacklogs)
            .HasForeignKey(backlog => backlog.DoneByUserId)
            .IsRequired(false);

        builder.HasOne(backlog => backlog.ConvertedBlogPost)
            .WithMany(blogPost => blogPost.Backlogs)
            .HasForeignKey(backlog => backlog.ConvertedBlogPostId)
            .IsRequired(false);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.Backlogs)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
