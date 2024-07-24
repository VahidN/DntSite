using DntSite.Web.Features.StackExchangeQuestions.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.EfConfig;

public class StackExchangeQuestionUserFileConfig : IEntityTypeConfiguration<StackExchangeQuestionUserFile>
{
    public void Configure(EntityTypeBuilder<StackExchangeQuestionUserFile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.Parent)
            .WithMany(file => file.UserFiles)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.StackExchangeQuestionUserFiles)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
