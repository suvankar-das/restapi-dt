using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly List<Movie> _movies = new();
    public Task<bool> CreateMovieAsync(Movie movie)
    {
        _movies.Add(movie);
        return Task.FromResult(true);
    }

    public Task<Movie?> GetMovieByIdAsync(Guid id)
    {
        var movie = _movies.SingleOrDefault(m => m.Id == id);
        return Task.FromResult(movie);
    }

    public Task<IEnumerable<Movie>> GetAllMoviesAsync()
    {
        return Task.FromResult(_movies.AsEnumerable());
    }

    public Task<bool> UpdateMovieAsync(Movie movie)
    {
        var movieIndex = _movies.FindIndex(x => x.Id == movie.Id);
        if (movieIndex == -1)
        {
            return Task.FromResult(false);
        }
        _movies[movieIndex] = movie;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteMovieAsync(Guid id)
    {
        var removedCount = _movies.RemoveAll(x => x.Id == id);
        var movieRemoved = removedCount > 0;
        return Task.FromResult(movieRemoved);
    }

    public Task<Movie?> GetMovieBySlugAsync(string slug)
    {
        var movie = _movies.SingleOrDefault(m => m.Slug == slug);
        return Task.FromResult(movie);
    }
}