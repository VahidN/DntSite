using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.EfConfig;

public class BlogPostConfig : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(entity => entity.Title).HasMaxLength(450).IsRequired();
        builder.Property(entity => entity.BriefDescription).HasMaxLength(450).IsRequired();
        builder.Property(entity => entity.OldUrl).HasMaxLength(1500).IsRequired(false);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BlogPosts)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
