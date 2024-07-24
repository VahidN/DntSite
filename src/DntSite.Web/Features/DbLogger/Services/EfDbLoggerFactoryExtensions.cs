namespace DntSite.Web.Features.DbLogger.Services;

public static class EfDbLoggerFactoryExtensions
{
    public static ILoggingBuilder AddDbLogger(this ILoggingBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddSingleton<ILoggerProvider, EfDbLoggerProvider>();

        return builder;
    }
}
