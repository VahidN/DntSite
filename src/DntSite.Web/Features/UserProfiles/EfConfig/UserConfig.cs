using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.UserProfiles.EfConfig;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Ignore(user => user.Tags);

        builder.Property(user => user.FriendlyName).HasMaxLength(450).IsRequired();
        builder.HasIndex(user => user.FriendlyName).IsUnique();

        builder.Property(user => user.UserName).HasMaxLength(450).IsRequired();
        builder.HasIndex(user => user.UserName).IsUnique();

        builder.Property(user => user.HashedPassword).HasMaxLength(1000).IsRequired();

        builder.Property(user => user.EMail).HasMaxLength(450).IsRequired();
        builder.HasIndex(user => user.EMail).IsUnique();

        builder.Property(user => user.RegistrationCode).HasMaxLength(255).IsRequired();

        builder.Property(user => user.HomePageUrl).HasMaxLength(1000).IsRequired(false);
        builder.Property(user => user.Photo).HasMaxLength(1000).IsRequired(false);
        builder.Property(user => user.Description).HasMaxLength(2000).IsRequired(false);
        builder.Property(user => user.Location).HasMaxLength(1000).IsRequired(false);
        builder.Property(user => user.SerialNumber).HasMaxLength(1000).IsRequired(false);

        builder.HasMany(user => user.UserAllowedCourses).WithMany(course => course.CourseAllowedUsers);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.Users)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        builder.HasOne(user => user.UserSocialNetwork)
            .WithOne(userSocialNetwork => userSocialNetwork.User)
            .HasForeignKey<UserSocialNetwork>(userSocialNetwork => userSocialNetwork.UserId);

        builder.HasMany(user => user.AdvertisementCommentReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.AdvertisementReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.BacklogCommentReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.BacklogReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.CourseCommentReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.CourseQuestionCommentReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.CourseQuestionReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.CourseReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.CourseTopicCommentReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.CourseTopicReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.DailyNewsItemCommentReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.DailyNewsItemReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.BlogPostCommentReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.BlogPostReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.PrivateMessageCommentReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.PrivateMessageReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.ProjectCommentReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.ProjectFaqCommentReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.ProjectFaqReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.ProjectIssueCommentReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.ProjectIssueReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.ProjectReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.ProjectReleaseCommentReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.ProjectReleaseReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.LearningPathCommentReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.LearningPathReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.SearchItemCommentReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.SearchItemReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.StackExchangeQuestionCommentReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.StackExchangeQuestionReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.SurveyCommentReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.SurveyReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.UserProfileCommentReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);

        builder.HasMany(user => user.UserProfileReactionsForUsers)
            .WithOne(entity => entity.ForUser)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);
    }
}
