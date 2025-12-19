using System.Text;
using DntSite.Web.Features.Advertisements.Models;
using DntSite.Web.Features.AppConfigs.Components;

namespace DntSite.Web.Features.Advertisements.Components;

public partial class AddJobOfferAdvertisementForm
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] [EditorRequired] public IList<string>? AutoCompleteDataList { get; set; }

    [Parameter] [EditorRequired] public EventCallback<WriteAdvertisementModel> OnValidSubmit { get; set; }

    [SupplyParameterFromForm(FormName = nameof(AddJobOfferAdvertisementForm))]
    public AddGeneralAdvertisementModel? Model { set; get; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Model ??= new AddGeneralAdvertisementModel();
    }

    private async Task PerformAsync()
    {
        if (!OnValidSubmit.HasDelegate || Model is null)
        {
            return;
        }

        await OnValidSubmit.InvokeAsync(new WriteAdvertisementModel
        {
            Title = $"آگهی استخدام {Model.JobTitle} در «{Model.OrganizationName}»",
            Body = GetArticleBody(),
            Tags = Model.Tags,
            DueDate = Model.DueDate.AddHours(Model.Hour ?? 0).AddMinutes(Model.Minute ?? 0)
        });
    }

    private string GetArticleBody()
    {
        if (Model is null)
        {
            return string.Empty;
        }

        List<List<string>> rows =
        [
            ["نام شرکت", Model.OrganizationName.Trim()], ["آدرس شرکت", Model.Address.Trim()],
            [
                "ارسال رزومه به",
                Model.SendResumeTo.Trim().Replace(oldValue: "\n", newValue: "<br>", StringComparison.Ordinal)
            ],
            [
                "آدرس وب سایت شرکت",
                Model.WebSiteUrl.IsValidUrl()
                    ? $"<a target='_blank' rel='nofollow' href='{Model.WebSiteUrl.Trim()}'>{Model.WebSiteUrl.Trim()}</a>"
                    : ""
            ],
            ["شماره تماس", $"<span dir='ltr'>{Model.Tel.Trim()}<span> ({Model.Name.Trim()})"],
            ["شرایط عمومی متقاضی", Model.GeneralConditions.Trim()],
            ["شرایط تخصصی متقاضی", Model.SpecialConditions.Trim()], ["اولویت‌ها", Model.SpecialPoints.Trim()],
            [
                "جنسیت متقاضی",
                $"{(Model.Genders?.Contains(Gender.Male) == true ? " «آقایان» " : "")}{(Model.Genders?.Contains(Gender.Female) == true ? " «خانم‌ها» " : "")}"
            ],
            [
                "نوع همکاری مورد نیاز",
                $"{(Model.JobTypes?.Contains(JobType.FullTime) == true ? " «تمام وقت» " : "")}{(Model.JobTypes?.Contains(JobType.PartTime) == true ? " «پاره وقت» " : "")}{(Model.JobTypes?.Contains(JobType.RemoteWorking) == true ? " «دور کاری» " : "")}"
            ],
            ["حداکثر سن متقاضی", $"{Model.MaxAge.ToPersianNumbers()} سال "],
            ["امتیازات حضور در مجموعه ما", Model.Benefits.Trim()]
        ];

        var sb = new StringBuilder();

        foreach (var row in rows)
        {
            sb.AppendLine(CultureInfo.InvariantCulture,
                $"<p>{row[index: 0].MakeItStrong()}: {row[index: 1]}</p><p></p>");
        }

        return sb.ToString();
    }
}
