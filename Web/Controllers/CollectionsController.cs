using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using zMovies.Web.Contracts.V1.RequestDTOs;
using zMovies.Web.Contracts.V1.ResponseDTOs;
using zMovies.Core.Interfaces;
using zMovies.Core.Entities;
using zMovies.Contracts.V1;
using Microsoft.AspNetCore.Http;
using zMovies.Web.Contracts.v1.ErrorResponses;

namespace zMovies.Web.Controllers
{
  [ApiController]
  [Produces("application/json")]
  [Consumes("application/json")]
  public class CollectionsController : ControllerBase
  {

    private readonly IMovieCollectionRepository collections;
    private readonly IUserRepository users;
    //private readonly IMovieCollectionItemRepository movieCollectionItems;
    private readonly IMapper mapper;

    public CollectionsController(IMovieCollectionRepository collections, /*IMovieCollectionItemRepository movieCollectionItems,*/ IUserRepository users, IMapper mapper)
    {
      //this.movieCollectionItems = movieCollectionItems;
      this.users = users;
      this.mapper = mapper;
      this.collections = collections;
    }
    
    /// <summary>
    /// Gets all the movie collections by user id
    /// </summary>
    /// <param name="uid">The user id</param>
    /// <returns>Returns all the movie collections by user id</returns>
    /// <response code="200">Returns all the movie collections belonging to a user</response>
    /// <response code="404">If the user id is invalid</response>     
    [HttpGet(ApiRoutes.Collections.GetAllCollectionsByUser)]
    [ProducesResponseType(typeof(IEnumerable<MovieCollectionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllCollectionsByUser([FromRoute] int uid)
    {
      User user = await this.users.GetById(uid);

      if (user == null)
        return NotFound(new ErrorResponse { Error = "Invalid uid"});

      IEnumerable<MovieCollection> allCollections = await this.collections.GetAllByUserId(uid);

      IEnumerable<MovieCollectionResponse> response = this.mapper.Map<IEnumerable<MovieCollectionResponse>>(allCollections);

      return Ok(response);
    }

    /// <summary>
    /// Gets a movie collection by id
    /// </summary>
    /// <param name="collectionId">The collection id</param>
    /// <returns>Returns a movie collection</returns>
    /// <response code="200">Returns a movie collection</response>
    /// <response code="404">If the collection id is invalid</response>     
    [HttpGet(ApiRoutes.Collections.GetCollectionById)]
    [ProducesResponseType(typeof(MovieCollectionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCollectionById([FromRoute] int collectionId)
    {
      MovieCollection foundCollection = await this.collections.GetById(collectionId);

      if (foundCollection == null)
        return NotFound(new ErrorResponse { Error = "Invalid collectionId"});
      
      MovieCollectionResponse response = this.mapper.Map<MovieCollectionResponse>(foundCollection);

      return Ok(response);
    }
    
    /// <summary>
    /// Creates a movie collection
    /// </summary>
    /// <param name="request">The request body</param>
    /// <returns>Returns the newly created movie collection</returns>
    /// <response code="201">Returns the newly created movie collection</response>
    /// <response code="400">If user id is invalid</response>
    /// <response code="409">If the movie collection name is already in use</response>  
    [HttpPost(ApiRoutes.Collections.CreateCollection)]
    [ProducesResponseType(typeof(MovieCollectionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateCollection([FromBody] CreateCollectionRequest request)
    {
      User user = await this.users.GetById(request.Uid);
      
      if (user == null) { return BadRequest(new ErrorResponse { Error = "Invalid User Id." }); }

      MovieCollection collectionToAdd = new MovieCollection
      {
        User = user,
        Name = request.Name,
        Description = request.Description
      };

      if ((await this.collections.CollectionNameExists(collectionToAdd))) { return Conflict(new ErrorResponse { Error = "Collection name already in use." }); }

      MovieCollection addedCollection = await this.collections.Add(collectionToAdd);

      MovieCollectionResponse response = this.mapper.Map<MovieCollectionResponse>(addedCollection);

      return CreatedAtAction("GetCollectionById", new {collectionId = response.Id }, response);
    }
    
    /// <summary>
    /// Updates a movie collection
    /// </summary>
    /// <param name="request">The request body</param>
    /// <returns>Returns the updated movie collection</returns>
    /// <response code="200">Returns the updated movie collection</response>
    /// <response code="404">If collection id is invalid</response> 
    [HttpPut(ApiRoutes.Collections.UpdateCollection)]
    [ProducesResponseType(typeof(MovieCollectionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCollection([FromBody] UpdateCollectionRequest request)
    {
      MovieCollection collectionToUpdate = await this.collections.GetById(request.Id);

      if (collectionToUpdate == null) { return NotFound(new ErrorResponse { Error = "Invalid collection Id." }); }

      collectionToUpdate.Name = request.Name;
      collectionToUpdate.Description = request.Description;

      MovieCollection movieCollection = await this.collections.Update(collectionToUpdate);
      
      MovieCollectionResponse response = this.mapper.Map<MovieCollectionResponse>(movieCollection);

      return Ok(response);
    }
    
    /// <summary>
    /// Deletes a movie collection
    /// </summary>
    /// <param name="collectionId">The collection id</param>
    /// <returns>Returns no content</returns>
    /// <response code="204">Returns no content</response>
    /// <response code="404">If collection id is invalid</response> 
    [HttpDelete(ApiRoutes.Collections.DeleteCollection)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCollection([FromRoute] int collectionId)
    {
      var collection = await this.collections.GetById(collectionId);
      
      if (collection == null)
        return NotFound(new ErrorResponse { Error = "Invalid collectionId"});

      await this.collections.Delete(collection);

      return NoContent();
    }

    /*
    /// <summary>
    /// Adds a movie to a movie collection
    /// </summary>
    /// <param name="request">The request body</param>
    /// <returns>Returns the newly created movie collection item</returns>
    /// <response code="201">Returns the newly created movie collection item</response>
    /// <response code="400">If user id is invalid</response> 
    /// <response code="409">If movie already exists on the movie collection</response> 
    [HttpPost(ApiRoutes.Collections.AddMovieToCollection)]
    [ProducesResponseType(typeof(MovieCollectionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddMovieToCollection([FromBody] AddMovieToCollectionRequest request)
    {
      MovieCollection collection = await this.collections.GetById(request.collectionId);

      if (collection == null) { return NotFound(new ErrorResponse { Error = "Invalid collection id" }); }

      MovieCollectionItem movieCollectionItem = new MovieCollectionItem
      {
        MovieId = request.MovieId,
        MovieCollectionId = collection.Id
      };

      if (await this.movieCollectionItems.Exists(request.MovieId, request.collectionId))
        return Conflict(new ErrorResponse { Error = "Movie already added on collection" });

      MovieCollectionItem movieOnCollection = await this.movieCollectionItems.Add(movieCollectionItem);

      MovieCollectionItemResponse response = this.mapper.Map<MovieCollectionItemResponse>(movieOnCollection);

      return CreatedAtAction("GetCollectionById", new { collectionId = response.MovieCollectionId }, response);
    }

    /// <summary>
    /// Deletes a movie from a movie collection
    /// </summary>
    /// <param name="collectionId">The collection id</param>
    /// <param name="movieId">The movie id</param>
    /// <returns>Returns no content</returns>
    /// <response code="204">Returns no content</response>
    /// <response code="404">If collection id or movie id is invalid</response> 
    [HttpDelete(ApiRoutes.Collections.DeleteMovieFromCollection)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMovieFromCollection([FromRoute] int collectionId, int movieId)
    {
      var item = await this.movieCollectionItems.GetById(movieId, collectionId);

      if (item == null)
        return NotFound(new ErrorResponse { Error = "Check collection id or movie id" });
        
      await this.movieCollectionItems.Delete(item);
      return NoContent();
    }*/
  }
}