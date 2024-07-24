using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Backlogs.Models;

public class ManageBacklogModel
{
    [Display(Name = "تخمین تکمیل (تعداد روز)")]
    [Required(ErrorMessage = "(عدد)")]
    [Range(1, 14, ErrorMessage = "بین 1 تا 14 روز")]
    public int? DaysEstimate { set; get; }

    [Display(Name = "شماره مطلب مرتبط منتشر شده")]
    public int? ConvertedBlogPostId { set; get; }

    public User? TakenByUser { set; get; }

    public int Id { set; get; }

    public DateTime? DoneDate { set; get; }

    public DateTime? StartDate { set; get; }
}
