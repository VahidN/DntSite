using DntSite.Web.Features.Searches.Entities;
using UAParser;

namespace DntSite.Web.Features.Searches.Models;

public class SearchItemModel
{
    public required SearchItem SearchItem { set; get; }

    public ClientInfo? ClientInfo { set; get; }
}
