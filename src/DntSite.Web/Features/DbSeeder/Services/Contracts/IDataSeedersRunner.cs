namespace DntSite.Web.Features.DbSeeder.Services.Contracts;

public interface IDataSeedersRunner : IScopedService
{
    void RunAllDataSeeders();
}
