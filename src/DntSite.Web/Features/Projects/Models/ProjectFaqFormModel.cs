namespace DntSite.Web.Features.Projects.Models;

public class ProjectFaqFormModel
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
    public string DescriptionText { set; get; } = default!;
}
