namespace DntSite.Web.Features.SideBar.Models;

public class CustomSidebarModel
{
    [Display(Name = "منوی سفارشی")]
    [Required(ErrorMessage = "(*)")]
    public string Description { set; get; } = default!;

    [Display(Name = "فعال است؟")] public bool IsPublic { set; get; }
}
