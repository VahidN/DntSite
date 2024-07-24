using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.EfConfig;

public class SurveyCommentReactionConfig : IEntityTypeConfiguration<SurveyCommentReaction>
{
    public void Configure(EntityTypeBuilder<SurveyCommentReaction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(reaction => reaction.Parent)
            .WithMany(entity => entity.Reactions)
            .HasForeignKey(reaction => reaction.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SurveyCommentReactions)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        builder.HasOne(entity => entity.ForUser)
            .WithMany(user => user.SurveyCommentReactionsForUsers)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);
    }
}
