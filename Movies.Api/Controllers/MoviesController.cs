using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Requests;
using Movies.Application.Responses;

namespace Movies.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpPost("movies")]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieRequest movieRequest)
        {
            var movieCreate = movieRequest.CreateMovieRequestToMovie();
            await _movieRepository.CreateMovieAsync(movieCreate);
            var response = movieCreate.CreateMovieResponseFromMovie();
            return Created($"/api/movies/{movieCreate.Id}", response);
        }
    }
}
