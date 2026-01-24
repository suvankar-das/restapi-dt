namespace Movies.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Movies
    {
        private const string MoviesBase = $"{ApiBase}/movies";
        public const string CreateMovie = MoviesBase;
        public const string GetMovieById = $"{MoviesBase}/{{slugOrId}}";
        public const string GetAllMovies = MoviesBase;
        public const string UpdateMovie = $"{MoviesBase}/{{id:guid}}";
        public const string DeleteMovie = $"{MoviesBase}/{{id:guid}}";
    }
}