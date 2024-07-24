using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Projects.Models;

public class NewProjectEmailModel : BaseEmailModel
{
    public required string Id { get; set; }

    public required string Title { get; set; }

    public required string DescriptionText { get; set; }

    public required string SourcecodeRepositoryUrl { get; set; }

    public required string License { get; set; }

    public required string RequiredDependencies { get; set; }

    public required string RelatedArticles { get; set; }

    public required string DevelopersDescription { get; set; }
}
