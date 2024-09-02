using System;

using Aram.BFF.Infrastructure.Common.Persistence;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Aram.BFF.Application.SubcutaneousTests.Common;

/// <summary>
///     In Subcutaneous tests we aren't testing integration with a real database,
///     so even if we weren't using SQLite we would use some in-memory database.
/// </summary>
public class SqlServerTestDatabase : IDisposable
{
    private SqlServerTestDatabase(string connectionString)
    {
        Connection = new SqliteConnection(connectionString);
    }

    public SqliteConnection Connection { get; }

    public void Dispose()
    {
        Connection.Close();
    }

    public static SqlServerTestDatabase CreateAndInitialize()
    {
        SqlServerTestDatabase testDatabase = new SqlServerTestDatabase("connectionString");


        testDatabase.InitializeDatabase();

        return testDatabase;
    }

    public void InitializeDatabase()
    {
        Connection.Open();
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(Connection)
            .Options;

        ApplicationDbContext context = new ApplicationDbContext(options, null!, null!);

        context.Database.EnsureCreated();
    }

    public void ResetDatabase()
    {
        Connection.Close();

        InitializeDatabase();
    }
}