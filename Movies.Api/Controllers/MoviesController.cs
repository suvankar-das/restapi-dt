using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Requests;
using Movies.Application.Responses;

namespace Movies.Api.Controllers
{
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpPost(ApiEndpoints.Movies.CreateMovie)]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieRequest movieRequest)
        {
            var movieCreate = movieRequest.CreateMovieRequestToMovie();
            await _movieRepository.CreateMovieAsync(movieCreate);
            var response = movieCreate.CreateMovieResponseFromMovie();
            return CreatedAtAction(nameof(GetMovieById), new { Id = movieCreate.Id }, response);
        }

        [HttpGet(ApiEndpoints.Movies.GetMovieById)]
        public async Task<IActionResult> GetMovieById([FromRoute] Guid id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            var response = movie.CreateMovieResponseFromMovie();
            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Movies.GetAllMovies)]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _movieRepository.GetAllMoviesAsync();
            var response = movies.CreateMovieResponsesFromMovies();
            return Ok(response);
        }

        [HttpPut(ApiEndpoints.Movies.UpdateMovie)]
        public async Task<IActionResult> UpdateMovie([FromRoute] Guid id, [FromBody] UpdateMovieRequest movieRequest)
        {
            var existingMovie = await _movieRepository.GetMovieByIdAsync(id);
            if (existingMovie == null)
            {
                return NotFound();
            }

            var updatedMovie = movieRequest.UpdateMovieRequestToMovie(id);
            await _movieRepository.UpdateMovieAsync(updatedMovie);
            var response = updatedMovie.CreateMovieResponseFromMovie();
            return Ok(response);
        }

        [HttpDelete(ApiEndpoints.Movies.DeleteMovie)]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            var movieToDelete = await _movieRepository.DeleteMovieAsync(id);
            if (!movieToDelete)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
