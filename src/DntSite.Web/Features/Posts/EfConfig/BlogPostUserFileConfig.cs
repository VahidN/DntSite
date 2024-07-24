using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.EfConfig;

public class BlogPostUserFileConfig : IEntityTypeConfiguration<BlogPostUserFile>
{
    public void Configure(EntityTypeBuilder<BlogPostUserFile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.Parent)
            .WithMany(file => file.UserFiles)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BlogPostUserFiles)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
