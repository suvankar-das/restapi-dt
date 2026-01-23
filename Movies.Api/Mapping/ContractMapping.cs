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
            YearOfRelease = movie.YearOfRelease,
            Genres = movie.Genres,
            Title = movie.Title
        };
    }
}