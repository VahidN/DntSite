using System.Text;
using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;

namespace DntSite.Web.Features.AppConfigs.Services;

public class DatabaseInfoService(IUnitOfWork uow) : IDatabaseInfoService
{
    private const float MaxAllowedFreePageToPagesCount = 0.5f;

    public async Task<DatabaseInfoModel> GetDatabaseInfoAsync()
    {
        var databaseSizeInBytes = await GetDatabaseSizeAsync();
        var databaseSizeWithoutFreePagesInBytes = await GetDatabaseSizeWithoutFreePagesInBytesAsync();

        return new DatabaseInfoModel
        {
            DatabaseFreeSpaceSizeInBytes = databaseSizeInBytes - databaseSizeWithoutFreePagesInBytes,
            DatabaseSizeInBytes = databaseSizeInBytes,
            CompileOptions = await GetCompileOptionsAsync(),
            DatabaseVersion = await GetDatabaseVersionAsync(),
            Pragmas = await GetPragmasAsync(),
            Tables = await GetSQLiteTablesAsync()
        };
    }

    public async Task<bool> NeedsShrinkDatabaseAsync()
    {
        var freePercent = await uow.SqlQuery<float>($"""
                                                     SELECT ((freelist_count * 1.0)/page_count) as value FROM
                                                     pragma_page_count(), pragma_freelist_count()
                                                     """)
            .FirstAsync();

        return freePercent > MaxAllowedFreePageToPagesCount;
    }

    public void ShrinkDatabase() => uow.ExecuteSqlRawCommand(query: "VACUUM;");

    private Task<string> GetDatabaseVersionAsync()
        => uow.SqlQuery<string>($"select sqlite_version() as value").FirstAsync();

    private Task<List<string>> GetCompileOptionsAsync()
        => uow.SqlQuery<string>($"PRAGMA compile_options;").ToListAsync();

    private Task<List<SQLitePragma>> GetPragmasAsync()
        => uow.SqlQuery<SQLitePragma>($"""
                                        with settings(pragma, value) as
                                            (
                                               select 'analysis_limit', * from pragma_analysis_limit
                                               union all select 'auto_vacuum', * from pragma_auto_vacuum
                                               union all select 'automatic_index', * from pragma_automatic_index
                                               union all select 'busy_timeout', * from pragma_busy_timeout
                                               union all select 'cache_size', * from pragma_cache_size
                                               union all select 'cache_spill', * from pragma_cache_spill
                                               union all select 'cell_size_check', * from pragma_cell_size_check
                                               union all select 'checkpoint_fullfsync', * from pragma_checkpoint_fullfsync
                                               union all select 'defer_foreign_keys', * from pragma_defer_foreign_keys
                                               union all select 'foreign_keys', * from pragma_foreign_keys
                                               union all select 'fullfsync', * from pragma_fullfsync
                                               union all select 'hard_heap_limit', * from pragma_hard_heap_limit
                                               union all select 'ignore_check_constraints', * from pragma_ignore_check_constraints
                                               union all select 'journal_mode', * from pragma_journal_mode
                                               union all select 'journal_size_limit', * from pragma_journal_size_limit
                                               union all select 'legacy_alter_table', * from pragma_legacy_alter_table
                                               union all select 'locking_mode', * from pragma_locking_mode
                                               union all select 'max_page_count', * from pragma_max_page_count
                                               union all select 'query_only', * from pragma_query_only
                                               union all select 'read_uncommitted', * from pragma_read_uncommitted
                                               union all select 'recursive_triggers', * from pragma_recursive_triggers
                                               union all select 'reverse_unordered_selects', * from pragma_reverse_unordered_selects
                                               union all select 'secure_delete', * from pragma_secure_delete
                                               union all select 'soft_heap_limit', * from pragma_soft_heap_limit
                                               union all select 'synchronous', * from pragma_synchronous
                                               union all select 'temp_store', * from pragma_temp_store
                                               union all select 'threads', * from pragma_threads
                                               union all select 'trusted_schema', * from pragma_trusted_schema
                                               union all select 'writable_schema', * from pragma_writable_schema
                                            )
                                       select * from settings
                                       """)
            .ToListAsync();

    private Task<long> GetDatabaseSizeWithoutFreePagesInBytesAsync()
        => uow.SqlQuery<long>($"""
                               SELECT (page_count - freelist_count) * page_size as value FROM
                               pragma_page_count(), pragma_freelist_count(), pragma_page_size()
                               """)
            .FirstAsync();

    private Task<long> GetDatabaseSizeAsync()
        => uow.SqlQuery<long>($"""
                               SELECT page_count * page_size as value FROM
                               pragma_page_count(), pragma_page_size()
                               """)
            .FirstAsync();

    private async Task<List<SQLiteTable>> GetSQLiteTablesAsync()
    {
        var tableNames = await uow.SqlQuery<string>($"SELECT name FROM sqlite_master WHERE type='table'").ToListAsync();
        var sb = new StringBuilder();
        sb.AppendLine(value: "with tables(name, RowsCount) as (");

        const string unionAll = " UNION all ";

        foreach (var tableName in tableNames)
        {
            sb.AppendFormat(CultureInfo.InvariantCulture,
                format: " SELECT '{0}' AS name, COUNT(*) AS RowsCount FROM '{0}' ", tableName);

            sb.Append(unionAll);
        }

        var sql = sb.ToString();
        sql = sql[..sql.LastIndexOf(unionAll, StringComparison.OrdinalIgnoreCase)];
        sql += ") select name, RowsCount from tables order by RowsCount desc";

        return await uow.SqlQueryRaw<SQLiteTable>(sql).ToListAsync();
    }
}
