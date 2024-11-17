using DntSite.Web.Features.Backlogs.Entities;

namespace DntSite.Web.Features.Backlogs.Models;

public class BacklogsListModel
{
    public IList<Backlog> Backlogs { set; get; } = [];

    public int AllItemsCount { set; get; }

    public int DoneItemsCount { set; get; }

    public int InProgressItemsCount { set; get; }

    public int NewItemsCount { set; get; }
}
