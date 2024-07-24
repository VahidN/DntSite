using DntSite.Web.Features.Backlogs.Entities;

namespace DntSite.Web.Features.Backlogs.EfConfig;

public class BacklogTagConfig : IEntityTypeConfiguration<BacklogTag>
{
    public void Configure(EntityTypeBuilder<BacklogTag> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(tag => tag.Name).HasMaxLength(450).IsRequired();
        builder.HasIndex(tag => tag.Name).IsUnique();

        builder.HasMany(tag => tag.AssociatedEntities).WithMany(backlog => backlog.Tags);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BacklogTags)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
