namespace DntSite.Web.Features.DbSeeder.Services.Contracts;

public interface IDataSeeder : IScopedService
{
    public int Order { set; get; }

    public void SeedData();
}
