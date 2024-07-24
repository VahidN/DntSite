namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntAuthorizedMessageBox
{
    [Parameter] public string AuthorizedMessage { set; get; } = "اولین نفری باشید که نظری را ارسال می\u200cکند!";

    [Parameter]
    public string NotAuthorizedMessage { set; get; } = "مهمان گرامی! برای ارسال نظر نیاز است وارد سایت شوید.";

    [Parameter] public required AlertType Type { get; set; } = AlertType.Info;
}
