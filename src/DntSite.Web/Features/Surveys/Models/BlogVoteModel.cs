using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.Models;

public class BlogVoteModel
{
    public Survey? CurrentItem { set; get; }

    public Survey? NextItem { set; get; }

    public Survey? PreviousItem { set; get; }
}
