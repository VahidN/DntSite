using DntSite.Web.Features.DbSeeder.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using Polly;

namespace DntSite.Web.Features.DbSeeder.Services;

public class DataSeedersRunner(IServiceProvider serviceProvider, IUnitOfWork uow) : IDataSeedersRunner
{
    public void RunAllDataSeeders()
    {
        MigrateDb();

        var seeders = serviceProvider.GetServices<IDataSeeder>().ToList();
        WriteLine(Invariant($"{DateTime.UtcNow:HH:mm:ss.fff} Running `{seeders.Count}` IDataSeeder(s)."));

        foreach (var seeder in seeders.OrderBy(dataSeeder => dataSeeder.Order))
        {
            WriteLine(Invariant($"{DateTime.UtcNow:HH:mm:ss.fff} Running IDataSeeder: `{seeder.GetType()}`"));
            seeder.SeedData();
        }
    }

    private void MigrateDb()
    {
        var retry = Policy.Handle<Exception>()
            .WaitAndRetry(new[]
            {
                TimeSpan.FromSeconds(value: 5), TimeSpan.FromSeconds(value: 10), TimeSpan.FromSeconds(value: 15)
            });

        retry.Execute(() =>
        {
            WriteLine(value: "Started MigrateDb");
            uow.Migrate(TimeSpan.FromMinutes(value: 7));
            WriteLine(value: "Finished MigrateDb");
        });
    }
}
