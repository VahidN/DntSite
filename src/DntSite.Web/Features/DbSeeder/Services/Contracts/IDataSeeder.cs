namespace DntSite.Web.Features.DbSeeder.Services.Contracts;

public interface IDataSeeder : IScopedService
{
    int Order { set; get; }

    void SeedData();
}
