using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.EfConfig;

public class DailyNewsItemUserFileConfig : IEntityTypeConfiguration<DailyNewsItemUserFile>
{
    public void Configure(EntityTypeBuilder<DailyNewsItemUserFile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.Parent)
            .WithMany(file => file.UserFiles)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.DailyNewsItemUserFiles)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
