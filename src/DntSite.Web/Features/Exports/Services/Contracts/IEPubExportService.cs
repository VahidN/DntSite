namespace DntSite.Web.Features.Exports.Services.Contracts;

public interface IEPubExportService : IScopedService
{
    Task StartAsync(CancellationToken cancellationToken = default);
}
