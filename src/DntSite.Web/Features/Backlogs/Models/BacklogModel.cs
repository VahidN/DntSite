﻿namespace DntSite.Web.Features.Backlogs.Models;

public class BacklogModel
{
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "متن عنوان خالی است")]
    [StringLength(450, MinimumLength = 1, ErrorMessage = "حداکثر طول عنوان پیام 450 حرف و حداقل آن 1 حرف می‌باشد")]
    public string Title { set; get; } = default!;

    [Display(Name = "لیست منابع و مآخذ پیشنهادی، جهت تکمیل بحث")]
    [Required(ErrorMessage = "متن منابع خالی است")]
    [MaxLength]
    public string Description { set; get; } = default!;

    [Display(Name = "گروه(ها)")]
    [Required(ErrorMessage = "لطفا حداقل یک گروه را وارد نمائید.")]
    [MinLength(1, ErrorMessage = "لطفا حداقل یک گروه را وارد کنید")]
    public IList<string> Tags { set; get; } = new List<string>();
}
