using Movies.Application.Models;

namespace Movies.Application.Repositories;

public interface IMovieRepository
{
    Task<bool> CreateMovieAsync(Movie movie);
    Task<Movie?> GetMovieByIdAsync(Guid id);
    Task<IEnumerable<Movie>> GetAllMoviesAsync();
    Task<bool> UpdateMovieAsync(Movie movie);
    Task<bool> DeleteMovieAsync(Guid id);
}