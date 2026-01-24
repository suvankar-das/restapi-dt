using Dapper;

namespace Movies.Application.Database;

public class DBInitializer
{
    private readonly IDBConnectionFactory _dbConnectionFactory;

    public DBInitializer(IDBConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync("""
                                      IF OBJECT_ID(N'dbo.Movies', N'U') IS NULL
                                      BEGIN
                                          CREATE TABLE dbo.Movies
                                          (
                                              Id UNIQUEIDENTIFIER NOT NULL
                                                  CONSTRAINT PK_Movies PRIMARY KEY
                                                  CONSTRAINT DF_Movies_Id DEFAULT NEWSEQUENTIALID(),
                                      
                                              Slug NVARCHAR(200) NOT NULL,
                                              Title NVARCHAR(200) NOT NULL,
                                              YearOfRelease INT NOT NULL
                                          );
                                      
                                          CREATE UNIQUE INDEX UX_Movies_Slug
                                          ON dbo.Movies (Slug);
                                      END
                                      """);
    }

}