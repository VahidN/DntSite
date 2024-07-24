using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.UserProfiles.EfConfig;

public class UserUsedPasswordConfig : IEntityTypeConfiguration<UserUsedPassword>
{
    public void Configure(EntityTypeBuilder<UserUsedPassword> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(userUsedPassword => userUsedPassword.User)
            .WithMany(user => user.UserUsedPasswords)
            .HasForeignKey(userUsedPassword => userUsedPassword.UserId)
            .IsRequired();
    }
}
