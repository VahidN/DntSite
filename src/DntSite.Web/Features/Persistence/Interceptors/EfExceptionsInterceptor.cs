using System.Data;
using System.Data.Common;
using System.Text;

namespace DntSite.Web.Features.Persistence.Interceptors;

public class EfExceptionsInterceptor(ILogger<EfExceptionsInterceptor> logger) : DbCommandInterceptor
{
    private readonly ILogger<EfExceptionsInterceptor> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
        => LogError(command, eventData);

    public override Task CommandFailedAsync(DbCommand command,
        CommandErrorEventData eventData,
        CancellationToken cancellationToken = default)
    {
        LogError(command, eventData);

        return Task.CompletedTask;
    }

    private void LogError(DbCommand? command, CommandErrorEventData? eventData)
    {
        if (command is null || eventData is null)
        {
            return;
        }

        var ex = eventData.Exception;
        var sqlData = LogSqlAndParameters(command);

        var contextualMessage =
            $"{sqlData}{Environment.NewLine}OriginalException:{Environment.NewLine}{ex.Demystify()} {Environment.NewLine}";

        _logger.LogErrorMessage(contextualMessage);
    }

    private static string LogSqlAndParameters(DbCommand command)
    {
        // -- Name: [Value] (Type = {}, Direction = {}, IsNullable = {}, Size = {}, Precision = {} Scale = {})
        var builder = new StringBuilder();

        var commandText = command.CommandText;

        builder.AppendFormat(CultureInfo.InvariantCulture, "{0}Command: {0}{1}", Environment.NewLine, commandText)
            .AppendLine();

        var parameters = command.Parameters.OfType<DbParameter>().ToList();

        if (parameters.Count > 0)
        {
            builder.AppendFormat(CultureInfo.InvariantCulture, "{0}Parameters: ", Environment.NewLine).AppendLine();
        }

        foreach (var parameter in parameters)
        {
            builder.Append("-- ")
                .Append(parameter.ParameterName)
                .Append(": '")
                .Append(parameter.Value is null || parameter.Value == DBNull.Value ? "null" : parameter.Value)
                .Append("' (Type = ")
                .Append(parameter.DbType);

            if (parameter.Direction != ParameterDirection.Input)
            {
                builder.Append(", Direction = ").Append(parameter.Direction);
            }

            if (!parameter.IsNullable)
            {
                builder.Append(", IsNullable = false");
            }

            if (parameter.Size != 0)
            {
                builder.Append(", Size = ").Append(parameter.Size);
            }

            if (((IDbDataParameter)parameter).Precision != 0)
            {
                builder.Append(", Precision = ").Append(((IDbDataParameter)parameter).Precision);
            }

            if (((IDbDataParameter)parameter).Scale != 0)
            {
                builder.Append(", Scale = ").Append(((IDbDataParameter)parameter).Scale);
            }

            builder.Append(')').Append(Environment.NewLine);
        }

        return builder.ToString();
    }
}
