using Aram.BFF.Infrastructure.Common.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Aram.BFF.Api.IntegrationTests.Common;

/// <summary>
///     We're using SQLite so no need to spin an actual database.
/// </summary>
public class SqlServerTestDatabase : IDisposable
{
    private SqlServerTestDatabase(string connectionString)
    {
        Connection = new SqlConnection(connectionString);
    }

    public SqlConnection Connection { get; }

    public void Dispose()
    {
        Connection.Close();
    }

    public static SqlServerTestDatabase CreateAndInitialize()
    {
        var testDatabase = new SqlServerTestDatabase("connectionString");

        testDatabase.InitializeDatabase();

        return testDatabase;
    }

    public void InitializeDatabase()
    {
        Connection.Open();
       var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(Connection)
            .Options;

        var context = new ApplicationDbContext(options, null!, null!);

        context.Database.EnsureCreated();
    }

    public void ResetDatabase()
    {
        Connection.Close();

        InitializeDatabase();
    }
}