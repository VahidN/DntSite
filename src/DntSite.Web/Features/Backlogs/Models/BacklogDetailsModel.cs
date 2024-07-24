using DntSite.Web.Features.Backlogs.Entities;

namespace DntSite.Web.Features.Backlogs.Models;

public class BacklogDetailsModel
{
    public Backlog? CurrentItem { set; get; }

    public Backlog? NextItem { set; get; }

    public Backlog? PreviousItem { set; get; }

    public BacklogsListModel? BacklogsListModel { set; get; }
}
