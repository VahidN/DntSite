namespace DntSite.Web.Features.StackExchangeQuestions.Models;

public class QuestionModel
{
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "لطفا عنوان را مشخص کنید")]
    [StringLength(maximumLength: 450, ErrorMessage = "حداکثر طول عنوان پیام 450 حرف می‌باشد")]
    public string Title { set; get; } = default!;

    [Display(Name = "توضیحات")]
    [MaxLength]
    [DataType(DataType.MultilineText)]
    [Required(ErrorMessage = "لطفا حداقل یک سطر توضیح را وارد نمائید.")]
    [RequiredHtmlContent(ErrorMessage = "لطفا حداقل یک سطر توضیح را وارد نمائید.")]
    [MinLength(length: 15, ErrorMessage = "لطفا حداقل یک سطر توضیح را وارد نمائید.")]
    public string Description { set; get; } = default!;

    [Display(Name = "گروه(ها)")]
    [Required(ErrorMessage = "لطفا حداقل یک گروه را وارد نمائید.")]
    [MinLength(length: 1, ErrorMessage = "لطفا حداقل یک گروه را وارد کنید")]
    public IList<string> Tags { set; get; } = [];
}
