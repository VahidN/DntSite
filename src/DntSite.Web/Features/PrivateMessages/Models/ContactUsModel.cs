namespace DntSite.Web.Features.PrivateMessages.Models;

public class ContactUsModel
{
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "متن عنوان خالی است")]
    [StringLength(maximumLength: 450, MinimumLength = 1,
        ErrorMessage = "حداکثر طول عنوان پیام 450 حرف و حداقل آن 1 حرف می‌باشد")]
    public string Title { set; get; } = default!;

    [Display(Name = "پیام")]
    [Required(ErrorMessage = "متن پیام خالی است")]
    [RequiredHtmlContent(ErrorMessage = "لطفا حداقل یک سطر توضیح را وارد نمائید.")]
    [MaxLength]
    [DataType(DataType.MultilineText)]
    public string DescriptionText { set; get; } = default!;
}
