namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

[ComplexType]
public class GuestUser
{
    [StringLength(450)] [Required] public string UserName { set; get; } = null!;

    [StringLength(1000)] public string? HomeUrl { set; get; }

    [StringLength(450)] public string? Email { set; get; }
}
