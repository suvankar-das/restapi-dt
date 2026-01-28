using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public MovieRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateMovieAsync(Movie movie)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(
            new CommandDefinition("""
                                  INSERT INTO Movies (Id, Title, Slug, YearOfRelease)
                                  VALUES (@Id, @Title, @Slug, @YearOfRelease)
                                  """, movie, transaction));

        if (result > 0)
        {
            foreach (var genre in movie.Genres)
            {
                await connection.ExecuteAsync(
                    new CommandDefinition("""
                                          INSERT INTO Genres (MovieId, Name)
                                          VALUES (@MovieId, @Genre)
                                          """, new { MovieId = movie.Id, Genre = genre }, transaction));
            }
        }

        transaction.Commit();
        return result > 0;

    }

    public async Task<Movie?> GetMovieByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
                                  SELECT m.Id,
                                  	   m.Slug,
                                  	   m.Title,
                                  	   m.YearOfRelease
                                  FROM Movies m
                                  WHERE m.Id=@Id
                                  """, new { Id = id }));

        if (movie is null)
        {
            return null;
        }

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                                  SELECT 
                                  	   g.Name
                                  FROM Genres g WHERE g.MovieId = @Id
                                  """, new { Id = movie.Id }));

        movie.Genres.AddRange(genres);
        return movie;
    }

    public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var resultSet = await connection.QueryAsync(
            new CommandDefinition("""
                                  SELECT m.Id,
                                  	   m.Slug,
                                  	   m.Title,
                                  	   m.YearOfRelease,
                                  	   STRING_AGG( g.Name, ',' ) AS genres
                                  FROM Movies m
                                  LEFT JOIN
                                  Genres g
                                  ON m.Id=g.MovieId
                                  GROUP BY Id,
                                  		 m.Slug,
                                  		 m.Title,
                                  		 m.YearOfRelease
                                  """));
        /* result ta erokom ashbe
        +--------------------------------------+---------------------+----------------+---------------+----------------+
        | Id                                   | Slug                | Title          | YearOfRelease | genres         |
        +--------------------------------------+---------------------+----------------+---------------+----------------+
        | BD6B7732-5E6F-4700-B4F9-28063B5DDA6B | nick-the-greek-2023 | Nick the Greek | 2023          | Comedy,Fantasy |
        +--------------------------------------+---------------------+----------------+---------------+----------------+
        */

        var movies = resultSet.Select(x => new Movie()
        {
            Id = x.Id,
            Title = x.Title,
            Genres = Enumerable.ToList(x.genres.Split(",")),
            YearOfRelease = x.YearOfRelease
        });

        return movies;
    }

    public async Task<bool> UpdateMovieAsync(Movie movie)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        await connection.ExecuteAsync(
           new CommandDefinition("""Delete from genres where MovieId=@Id""", new { Id = movie.Id }, transaction));

        foreach (var genre in movie.Genres)
        {
            await connection.ExecuteAsync(
                new CommandDefinition("""
                                      INSERT INTO Genres (MovieId, Name)
                                      VALUES (@MovieId, @Genre)
                                      """, new { MovieId = movie.Id, Genre = genre }, transaction));
        }


        var result = await connection.ExecuteAsync(
            new CommandDefinition("""
                                  UPDATE Movies 
                                  SET
                                  	Slug=@Slug,
                                  	Title=@Title,
                                  	YearOfRelease=@YearOfRelease
                                  WHERE Id=@Id;
                                  """, movie, transaction));
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> DeleteMovieAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        await connection.ExecuteAsync(
            new CommandDefinition("""Delete from genres where MovieId=@Id""", new { Id = id }, transaction));

        var result = await connection.ExecuteAsync(
            new CommandDefinition("""Delete from Movies where Id=@Id""", new { Id = id }, transaction));
        transaction.Commit();
        return result > 0;
    }

    public async Task<Movie?> GetMovieBySlugAsync(string slug)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
                                  SELECT m.Id,
                                  	   m.Slug,
                                  	   m.Title,
                                  	   m.YearOfRelease
                                  FROM Movies m
                                  WHERE m.Slug=@slug
                                  """, new { slug = slug }));

        if (movie is null)
        {
            return null;
        }

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                                  SELECT
                                  	   g.Name
                                  FROM Genres g WHERE g.MovieId = @Id
                                  """, new { Id = movie.Id }));

        movie.Genres.AddRange(genres);
        return movie;
    }

    public async Task<bool> CheckIfExists(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                """
                SELECT COUNT(Id) FROM Movies m WHERE m.Id =@Id 
                """, new { Id = id }));
        return result;
    }
}