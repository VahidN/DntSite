using DntSite.Web.Features.Searches.Entities;

namespace DntSite.Web.Features.Searches.EfConfig;

public class SearchItemUserFileConfig : IEntityTypeConfiguration<SearchItemUserFile>
{
    public void Configure(EntityTypeBuilder<SearchItemUserFile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.Parent)
            .WithMany(file => file.UserFiles)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SearchItemUserFiles)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
