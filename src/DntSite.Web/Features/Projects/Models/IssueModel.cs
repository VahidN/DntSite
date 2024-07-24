namespace DntSite.Web.Features.Projects.Models;

public class IssueModel
{
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "متن عنوان خالی است")]
    [StringLength(maximumLength: 450, MinimumLength = 1,
        ErrorMessage = "حداکثر طول عنوان پیام 450 حرف و حداقل آن 1 حرف می‌باشد")]
    public string Title { set; get; } = default!;

    [Display(Name = "توضیحات")]
    [Required(ErrorMessage = "توضیحات خالی است")]
    [MaxLength]
    [DataType(DataType.MultilineText)]
    public string Description { set; get; } = default!;

    [Display(Name = "شماره نگارش مرتبط")]
    [Required(ErrorMessage = "متن شماره نگارش خالی است")]
    [StringLength(maximumLength: 450, MinimumLength = 1,
        ErrorMessage = "حداکثر طول شماره نگارش 450 حرف و حداقل آن 1 حرف می‌باشد")]
    public string RevisionNumber { set; get; } = default!;

    [Display(Name = "میزان اهمیت")]
    [Required(ErrorMessage = "لطفا میزان اهمیت را تکمیل کنید")]
    public int IssuePriorityId { set; get; }

    [Display(Name = "نوع درخواست")]
    [Required(ErrorMessage = "لطفا نوع درخواست را تکمیل کنید")]
    public int IssueTypeId { set; get; }
}
