using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using zMovies.Web.Contracts.V1.RequestDTOs;
using zMovies.Web.Contracts.V1.ResponseDTOs;
using zMovies.Core.Entities;
using zMovies.Core.Interfaces;
using zMovies.Contracts.V1;
using Microsoft.AspNetCore.Http;

namespace Movies.Web.Controllers
{
  [Produces("application/json")]
  [ApiController]
  [Route("")]
  public class RatingsController : ControllerBase
  {
    private readonly IMovieRatingRepository movieRatings;
    private readonly IUserRepository users;
    private readonly IRatingRepository ratingValues;
    private readonly IMapper mapper;

    public RatingsController(IMovieRatingRepository ratingsRepo, IUserRepository users, IRatingRepository ratings, IMapper mapper)
    {
      this.ratingValues = ratings;
      this.mapper = mapper;
      this.users = users;
      this.movieRatings = ratingsRepo;
    }
    
    [HttpGet(ApiRoutes.Ratings.GetAllRatingsByUser)]
    public async Task<IActionResult> GetAllRatingsByUser([FromRoute] int uid)
    {
      User user = await this.users.GetById(uid);

      if (user == null)
        return NotFound(new { error = "Invalid uid" });

      IEnumerable<MovieRating> ratings = await this.movieRatings.GetAllByUserId(uid);

      IEnumerable<MovieRatingResponse> response = this.mapper.Map<IEnumerable<MovieRatingResponse>>(ratings);

      return Ok(response);
    }

    [HttpGet(ApiRoutes.Ratings.GetAllRatingsByMovie)]
    public async Task<IActionResult> GetAllRatingsByMovie([FromRoute] int movieId)
    {
      IEnumerable<MovieRating> ratings = await this.movieRatings.GetAllByMovieId(movieId);
      
      IEnumerable<MovieRatingResponse> response = this.mapper.Map<IEnumerable<MovieRatingResponse>>(ratings);

      return Ok(response);
    }

    [HttpPost(ApiRoutes.Ratings.Create)]
    public async Task<IActionResult> CreateRating([FromBody] CreateRatingRequest request)
    {
        User user = await this.users.GetById(request.Uid);
        Rating rating = await this.ratingValues.GetByValue(request.Value);

        if (user == null) { return BadRequest(new { error = "Invalid uid" }); }
        if (rating == null) { return BadRequest(new { error = "Invalid Rating Value" }); }

        MovieRating userRating = new MovieRating 
        {
            UserId = user.Id,
            MovieId = request.MovieId,
            RatingId = rating.Id,
            Review = request.Review
        };

        if (await this.movieRatings.Exists(request.Uid, request.MovieId)) { return Conflict(new { error = "Rating already exists" }); }

        MovieRating ratingAdded = await this.movieRatings.Add(userRating);
        
        MovieRatingResponse response = this.mapper.Map<MovieRatingResponse>(ratingAdded);

        return CreatedAtAction("GetAllRatingsByMovie", new { movieId = response.MovieId }, response);
    }

    [HttpPut(ApiRoutes.Ratings.Update)]
    public async Task<IActionResult> UpdateRating(UpdateRatingRequest request)
    {
        User user = await this.users.GetById(request.Uid);
        Rating rating = await this.ratingValues.GetByValue(request.Value);

        if (user == null) { return BadRequest(new { error = "Invalid uid" }); }
        if (rating == null) { return BadRequest(new { error = "Invalid Rating Value" }); }
        
        MovieRating updatedRating = new MovieRating {
          UserId = user.Id,
          MovieId = request.MovieId,
          RatingId = rating.Id,
          Review = request.Review
        };

        updatedRating = await this.movieRatings.Update(updatedRating);
        
        if (updatedRating == null) { return BadRequest(new { error = "Movie rating does not exist" }); }

        MovieRatingResponse response = this.mapper.Map<MovieRatingResponse>(updatedRating);

        return Ok(response);
    }

    [HttpDelete(ApiRoutes.Ratings.Delete)]
    public async Task<IActionResult> DeleteRating([FromRoute] int uid, int movieId)
    {
      var movieRating = await this.movieRatings.GetById(uid, movieId);
      
      if (movieRating == null)
        return NotFound(new {error = "Rating does not exist"});

      await this.movieRatings.Delete(movieRating);

      return NoContent();
    }

  }
}