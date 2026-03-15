using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Posts.Components;

public partial class ShowMainAuthorVisitDate
{
    [Parameter] [EditorRequired] public User? MainPostAuthor { set; get; }

    private AlertType LastVisitAlertType => MainPostAuthor?.LastVisitDateTime?.GetAge() switch
    {
        < 1 => AlertType.Info,
        >= 1 and <= 2 => AlertType.Warning,
        _ => AlertType.Danger
    };
}
