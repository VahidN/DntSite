using DntSite.Web.Features.StackExchangeQuestions.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.EfConfig;

public class StackExchangeQuestionTagConfig : IEntityTypeConfiguration<StackExchangeQuestionTag>
{
    public void Configure(EntityTypeBuilder<StackExchangeQuestionTag> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(tag => tag.Name).HasMaxLength(450).IsRequired();
        builder.HasIndex(tag => tag.Name).IsUnique();

        builder.HasMany(entity => entity.AssociatedEntities).WithMany(@base => @base.Tags);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.StackExchangeQuestionTags)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
