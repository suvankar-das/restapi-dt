using System.Data;
using Microsoft.Data.SqlClient;

namespace Movies.Application.Database;

public interface IDBConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}

public class SqlServerConnectionFactory : IDBConnectionFactory
{
    private readonly string _connectionString;

    public SqlServerConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}