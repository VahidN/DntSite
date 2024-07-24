using System.Text;
using DntSite.Web.Features.Advertisements.Models;

namespace DntSite.Web.Features.Advertisements.Components;

public partial class AddNewProjectAdvertisementForm
{
    [Parameter] [EditorRequired] public IList<string>? AutoCompleteDataList { get; set; }

    [Parameter] [EditorRequired] public EventCallback<WriteAdvertisementModel> OnValidSubmit { get; set; }

    [SupplyParameterFromForm(FormName = nameof(AddNewProjectAdvertisementForm))]
    public AddNewProjectAdvertisementModel Model { set; get; } = new();

    private async Task PerformAsync()
    {
        if (!OnValidSubmit.HasDelegate)
        {
            return;
        }

        await OnValidSubmit.InvokeAsync(new WriteAdvertisementModel
        {
            Title = "آگهی درخواست انجام پروژه",
            Body = GetArticleBody(),
            Tags = Model.Tags,
            DueDate = Model.DueDate.AddHours(Model.Hour ?? 0).AddMinutes(Model.Minute ?? 0)
        });
    }

    private string GetArticleBody()
    {
        List<List<string>> rows =
        [
            ["توضیحات عمومی پروژه", Model.GeneralConditions.Trim()],
            ["فناوری‌های مدنظر جهت انجام پروژه", Model.SpecialConditions.Trim()],
            ["شماره تماس", $"<span dir='ltr'>{Model.Tel.Trim()}<span> ({Model.Name.Trim()})"],
            ["ارسال رزومه به", Model.SendResumeTo.Trim()]
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
