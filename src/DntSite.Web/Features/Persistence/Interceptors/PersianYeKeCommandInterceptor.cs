using System.Data;
using System.Data.Common;

namespace DntSite.Web.Features.Persistence.Interceptors;

public class PersianYeKeCommandInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        ApplyCorrectYeKe(command);

        return result;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = new())
    {
        ApplyCorrectYeKe(command);

        return ValueTask.FromResult(result);
    }

    public override InterceptionResult<int> NonQueryExecuting(DbCommand command,
        CommandEventData eventData,
        InterceptionResult<int> result)
    {
        ApplyCorrectYeKe(command);

        return result;
    }

    public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command,
        CommandEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        ApplyCorrectYeKe(command);

        return ValueTask.FromResult(result);
    }

    public override InterceptionResult<object> ScalarExecuting(DbCommand command,
        CommandEventData eventData,
        InterceptionResult<object> result)
    {
        ApplyCorrectYeKe(command);

        return result;
    }

    public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command,
        CommandEventData eventData,
        InterceptionResult<object> result,
        CancellationToken cancellationToken = new())
    {
        ApplyCorrectYeKe(command);

        return ValueTask.FromResult(result);
    }

    [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities",
        Justification = "`ApplyCorrectYeKe()` method is safe.")]
    private static void ApplyCorrectYeKe(DbCommand command)
    {
        if (command is null)
        {
            return;
        }

        command.CommandText = command.CommandText.ApplyCorrectYeKe();

        foreach (DbParameter parameter in command.Parameters)
        {
            switch (parameter.DbType)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.Xml:
                    if (parameter.Value is not DBNull && parameter.Value is string)
                    {
                        var value = Convert.ToString(parameter.Value, CultureInfo.InvariantCulture);

                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            parameter.Value = value.ApplyCorrectYeKe();
                        }
                    }

                    break;
            }
        }
    }
}
