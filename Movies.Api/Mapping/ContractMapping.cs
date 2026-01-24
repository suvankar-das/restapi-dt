using Movies.Application.Models;
using Movies.Application.Requests;
using Movies.Application.Responses;

namespace Movies.Api.Mapping;

public static class ContractMapping
{
    public static Movie CreateMovieRequestToMovie(this CreateMovieRequest createMovieRequest)
    {
        return new Movie()
        {
            Id = Guid.NewGuid(),
            Genres = createMovieRequest.Genres.ToList(),
            Title = createMovieRequest.Title,
            YearOfRelease = createMovieRequest.YearOfRelease
        };
    }

    public static MovieResponse CreateMovieResponseFromMovie(this Movie movie)
    {
        return new MovieResponse()
        {
            Id = movie.Id,
            Slug = movie.Slug,
            YearOfRelease = movie.YearOfRelease,
            Genres = movie.Genres,
            Title = movie.Title
        };
    }

    public static MoviesResponse CreateMovieResponsesFromMovies(this IEnumerable<Movie> movies)
    {
        return new MoviesResponse()
        {
            Items = movies.Select(CreateMovieResponseFromMovie)
        };
    }

    public static Movie UpdateMovieRequestToMovie(this UpdateMovieRequest existingMovie , Guid Id)
    {
        return new Movie()
        {
            Id = Id,
            Genres = existingMovie.Genres.ToList(),
            Title = existingMovie.Title,
            YearOfRelease = existingMovie.YearOfRelease
        };
    }
}