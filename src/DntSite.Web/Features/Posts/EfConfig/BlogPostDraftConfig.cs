using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.EfConfig;

public class BlogPostDraftConfig : IEntityTypeConfiguration<BlogPostDraft>
{
    public void Configure(EntityTypeBuilder<BlogPostDraft> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(entity => entity.Title).HasMaxLength(450).IsRequired();
        builder.Property(entity => entity.Tags).HasMaxLength(1500).IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BlogPostDrafts)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
