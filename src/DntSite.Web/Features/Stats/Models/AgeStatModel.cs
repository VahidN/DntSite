using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Stats.Models;

public class AgeStatModel
{
    private const char RleChar = (char)0x202B;

    public int UsersCount { set; get; }

    public int AverageAge { set; get; }

    public User? MaxAgeUser { set; get; }

    public User? MinAgeUser { set; get; }

    public string MinMax
    {
        get
        {
            if (MinAgeUser?.DateOfBirth == null || MaxAgeUser?.DateOfBirth == null)
            {
                return string.Empty;
            }

            return string.Create(CultureInfo.InvariantCulture,
                    $"{RleChar}جوان\u200cترین عضو: {MinAgeUser.FriendlyName} ({MinAgeUser.DateOfBirth.Value.GetAge()}) و مسن\u200cترین عضو: {MaxAgeUser.FriendlyName} ({MaxAgeUser.DateOfBirth.Value.GetAge()})؛ در بین {UsersCount} نفر (که اطلاعات تاریخ تولد خود را تکمیل کرده‌اند).")
                .ToPersianNumbers();
        }
    }
}
