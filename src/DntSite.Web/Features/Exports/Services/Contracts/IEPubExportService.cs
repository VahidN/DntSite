namespace DntSite.Web.Features.Exports.Services.Contracts;

public interface IEPubExportService : IScopedService
{
    Task StartAsync(bool uploadFile, bool deleteFileAtEnd, CancellationToken cancellationToken = default);
}
