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
using zMovies.Web.Contracts.V1.ErrorResponses;

namespace Movies.Web.Controllers
{
  [ApiController]
  [Produces("application/json")]
  [Consumes("application/json")]
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
    
    /// <summary>
    /// Gets all ratings by user id
    /// </summary>
    /// <param name="uid">The user id</param>
    /// <returns>Returns all the movie ratings by user id</returns>
    /// <response code="200">Returns all the movie ratings by user id</response>
    /// <response code="404">If the user id is invalid</response>   
    [HttpGet(ApiRoutes.Ratings.GetAllRatingsByUser)]
    [ProducesResponseType(typeof(IEnumerable<MovieRatingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllRatingsByUser([FromRoute] int uid)
    {
      User user = await this.users.GetById(uid);

      if (user == null)
        return NotFound(new ErrorResponse { Error = "Invalid uid" });

      IEnumerable<MovieRating> ratings = await this.movieRatings.GetAllByUserId(uid);

      IEnumerable<MovieRatingResponse> response = this.mapper.Map<IEnumerable<MovieRatingResponse>>(ratings);

      return Ok(response);
    }

    /// <summary>
    /// Gets all ratings by movie id
    /// </summary>
    /// <param name="movieId">The movie id</param>
    /// <returns>Returns all the movie ratings by movie id</returns>
    /// <response code="200">Returns all the movie ratings by movie id</response>
    [HttpGet(ApiRoutes.Ratings.GetAllRatingsByMovie)]
    [ProducesResponseType(typeof(IEnumerable<MovieRatingResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRatingsByMovie([FromRoute] int movieId)
    {
      IEnumerable<MovieRating> ratings = await this.movieRatings.GetAllByMovieId(movieId);
      
      IEnumerable<MovieRatingResponse> response = this.mapper.Map<IEnumerable<MovieRatingResponse>>(ratings);

      return Ok(response);
    }

    /// <summary>
    /// Creates a movie rating
    /// </summary>
    /// <param name="request">The request body</param>
    /// <returns>Returns the newly created movie rating</returns>
    /// <response code="201">Returns the newly created movie rating</response>
    /// <response code="400">If user id or rating value is invalid</response>
    /// <response code="409">If the movie rating already exists</response>
    [HttpPost(ApiRoutes.Ratings.Create)]
    [ProducesResponseType(typeof(MovieRatingResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateRating([FromBody] CreateRatingRequest request)
    {
        User user = await this.users.GetById(request.Uid);
        Rating rating = await this.ratingValues.GetByValue(request.Value);

        if (user == null) { return BadRequest(new ErrorResponse { Error = "Invalid uid" }); }
        if (rating == null) { return BadRequest(new ErrorResponse { Error = "Invalid Rating Value" }); }

        MovieRating userRating = new MovieRating 
        {
            UserId = user.Id,
            MovieId = request.MovieId,
            RatingId = rating.Id,
            Review = request.Review
        };

        if (await this.movieRatings.Exists(request.Uid, request.MovieId)) { return Conflict(new ErrorResponse { Error = "Rating already exists" }); }

        MovieRating ratingAdded = await this.movieRatings.Add(userRating);
        
        MovieRatingResponse response = this.mapper.Map<MovieRatingResponse>(ratingAdded);

        return CreatedAtAction("GetAllRatingsByMovie", new { movieId = response.MovieId }, response);
    }

    /// <summary>
    /// Updates a movie rating
    /// </summary>
    /// <param name="request">The request body</param>
    /// <returns>Returns the updated movie rating</returns>
    /// <response code="200">Returns the updated movie rating</response>
    /// <response code="400">If the uid or movie is invalid</response>
    /// <response code="404">If movie rating does not exist</response>
    [HttpPut(ApiRoutes.Ratings.Update)]
    [ProducesResponseType(typeof(MovieRatingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRating(UpdateRatingRequest request)
    {
        User user = await this.users.GetById(request.Uid);
        Rating rating = await this.ratingValues.GetByValue(request.Value);
        MovieRating ratingToUpdate = await this.movieRatings.GetById(request.Uid, request.MovieId);
        
        if (ratingToUpdate == null) { return NotFound(new ErrorResponse { Error = "Movie rating does not exist" }); }
        if (user == null) { return BadRequest(new ErrorResponse { Error = "Invalid uid" }); }
        if (rating == null) { return BadRequest(new ErrorResponse { Error = "Invalid Rating Value" }); }

        ratingToUpdate.RatingId = rating.Id;
        ratingToUpdate.Review = request.Review;

        ratingToUpdate = await this.movieRatings.Update(ratingToUpdate);

        MovieRatingResponse response = this.mapper.Map<MovieRatingResponse>(ratingToUpdate);

        return Ok(response);
    }

    /// <summary>
    /// Deletes a movie rating
    /// </summary>
    /// <param name="uid">The user id</param>
    /// <param name="movieId">The movie id</param>
    /// <returns>Returns no content</returns>
    /// <response code="204">Returns no content</response>
    /// <response code="404">If rating does not exist</response> 
    [HttpDelete(ApiRoutes.Ratings.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRating([FromRoute] int uid, int movieId)
    {
      var movieRating = await this.movieRatings.GetById(uid, movieId);
      
      if (movieRating == null)
        return NotFound(new ErrorResponse { Error = "Rating does not exist"});

      await this.movieRatings.Delete(movieRating);

      return NoContent();
    }

  }
}