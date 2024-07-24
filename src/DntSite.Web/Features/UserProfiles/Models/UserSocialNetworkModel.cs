namespace DntSite.Web.Features.UserProfiles.Models;

public class UserSocialNetworkModel
{
    private const string StartsWithHttp = @"^(?!((http|https|ftp):\/\/)).*$";
    private const string DontEnterHttp = "لطفا آدرس کامل را وارد نکنید. صرفا نام یا آی‌دی پروفایل کفایت می‌کند.";

    [RegularExpression(StartsWithHttp, ErrorMessage = DontEnterHttp)]
    [Display(Name = "نام پروفایل توئیتر")]
    public string? TwitterName { set; get; }

    [RegularExpression(StartsWithHttp, ErrorMessage = DontEnterHttp)]
    [Display(Name = "نام پروفایل فیس‌ بوک")]
    public string? FacebookName { set; get; }

    [RegularExpression(StartsWithHttp, ErrorMessage = DontEnterHttp)]
    [Display(Name = "نام پروفایل لینکداین")]
    public string? LinkedInProfileId { set; get; }

    [RegularExpression(StartsWithHttp, ErrorMessage = DontEnterHttp)]
    [Display(Name = "نام پروفایل تلگرام")]
    public string? TelegramId { set; get; }

    [Display(Name = "شماره پروفایل استک اور فلو")]
    [RegularExpression(pattern: "\\d+", ErrorMessage = "لطفا عدد وارد کنید")]
    public string? StackOverflowId { set; get; }

    [RegularExpression(StartsWithHttp, ErrorMessage = DontEnterHttp)]
    [Display(Name = "نام پروفایل گیت‌هاب")]
    public string? GithubId { set; get; }

    [RegularExpression(StartsWithHttp, ErrorMessage = DontEnterHttp)]
    [Display(Name = "نام پروفایل نیوگت")]
    public string? NugetId { set; get; }

    [RegularExpression(StartsWithHttp, ErrorMessage = DontEnterHttp)]
    [Display(Name = "نام پروفایل کد پروجکت")]
    public string? CodeProjectId { set; get; }

    [RegularExpression(StartsWithHttp, ErrorMessage = DontEnterHttp)]
    [Display(Name = "نام پروفایل سورس‌ فورج")]
    public string? SourceforgeId { set; get; }

    [RegularExpression(StartsWithHttp, ErrorMessage = DontEnterHttp)]
    [Display(Name = "نام پروفایل کافیته")]
    public string? CoffeebedeId { set; get; }

    [RegularExpression(StartsWithHttp, ErrorMessage = DontEnterHttp)]
    [Display(Name = "نام پروفایل یوتیوب")]
    public string? YouTubeId { set; get; }

    [RegularExpression(StartsWithHttp, ErrorMessage = DontEnterHttp)]
    [Display(Name = "نام پروفایل ردیت")]
    public string? RedditId { set; get; }
}
