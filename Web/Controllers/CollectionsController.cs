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

namespace zMovies.Web.Controllers
{
  [Route("")]
  [ApiController]
  public class CollectionsController : ControllerBase
  {

    private readonly IMovieCollectionRepository collections;
    private readonly IUserRepository users;
    private readonly IMovieCollectionItemRepository movieCollectionItems;
    private readonly IMapper mapper;

    public CollectionsController(IMovieCollectionRepository collections, IMovieCollectionItemRepository movieCollectionItems, IUserRepository users, IMapper mapper)
    {
      this.movieCollectionItems = movieCollectionItems;
      this.users = users;
      this.mapper = mapper;
      this.collections = collections;
    }

    [HttpGet(ApiRoutes.Collections.GetAllCollectionsByUser)]
    public async Task<IActionResult> GetAllCollectionsByUser([FromRoute] int uid)
    {
      User user = await this.users.GetById(uid);

      if (user == null)
        return NotFound(new {error = "Invalid uid"});

      IEnumerable<MovieCollection> allCollections = await this.collections.GetAllByUserId(uid);

      IEnumerable<MovieCollectionResponse> response = this.mapper.Map<IEnumerable<MovieCollectionResponse>>(allCollections);

      return Ok(response);
    }

    [HttpGet(ApiRoutes.Collections.GetCollectionById)]
    public async Task<IActionResult> GetCollectionById([FromRoute] int collectionId)
    {
      MovieCollection foundCollection = await this.collections.GetById(collectionId);

      if (foundCollection == null)
        return NotFound(new {error = "Invalid collectionId"});
      
      MovieCollectionResponse response = this.mapper.Map<MovieCollectionResponse>(foundCollection);

      return Ok(foundCollection);
    }

    [HttpPost(ApiRoutes.Collections.CreateCollection)]
    public async Task<IActionResult> CreateCollection([FromBody] CreateCollectionRequest request)
    {
      User user = await this.users.GetById(request.Uid);

      MovieCollection collectionToAdd = new MovieCollection
      {
        User = user,
        Name = request.Name,
        Description = request.Description
      };

      if (user == null) { return BadRequest(new { error = "Invalid User Id." }); }
      if ((await this.collections.CollectionNameExists(collectionToAdd))) { return Ok("Collection name already exists"); }

      MovieCollection addedCollection = await this.collections.Add(collectionToAdd);

      MovieCollectionResponse response = this.mapper.Map<MovieCollectionResponse>(addedCollection);

      return CreatedAtAction("GetCollectionById", new {collectionId = response.Id }, response);
    }

    [HttpPut(ApiRoutes.Collections.UpdateCollection)]
    public async Task<IActionResult> UpdateCollection([FromBody] UpdateCollectionRequest request)
    {
      MovieCollection collectionToUpdate = await this.collections.GetById(request.Id);

      if (collectionToUpdate == null) { return NotFound(new { error = "Invalid collection Id." }); }

      collectionToUpdate.Name = request.Name;
      collectionToUpdate.Description = request.Description;

      MovieCollection movieCollection = await this.collections.Update(collectionToUpdate);
      
      MovieCollectionResponse response = this.mapper.Map<MovieCollectionResponse>(movieCollection);

      return Ok(response);
    }

    [HttpDelete(ApiRoutes.Collections.DeleteCollection)]
    public async Task<IActionResult> DeleteCollection([FromRoute] int collectionId)
    {
      var collection = await this.collections.GetById(collectionId);
      
      if (collection == null)
        return NotFound(new { error = "Invalid collectionId"});

      await this.collections.Delete(collection);

      return NoContent();
    }

    [HttpPost(ApiRoutes.Collections.AddMovieToCollection)]
    public async Task<IActionResult> AddMovieToCollection([FromBody] AddMovieToCollectionRequest request)
    {
      MovieCollection collection = await this.collections.GetById(request.collectionId);

      if (collection == null) { return NotFound(new { error = "Invalid collection id" }); }

      MovieCollectionItem movieCollectionItem = new MovieCollectionItem
      {
        MovieId = request.MovieId,
        MovieCollectionId = collection.Id
      };

      if (await this.movieCollectionItems.Exists(request.MovieId, request.collectionId))
        return Ok(new { error = "Movie already added on collection" });

      MovieCollectionItem movieOnCollection = await this.movieCollectionItems.Add(movieCollectionItem);

      MovieCollectionItemResponse response = this.mapper.Map<MovieCollectionItemResponse>(movieOnCollection);

      return CreatedAtAction("GetCollectionById", new { collectionId = response.MovieCollectionId }, response);
    }

    [HttpDelete(ApiRoutes.Collections.DeleteMovieFromCollection)]
    public async Task<IActionResult> DeleteMovieFromCollection([FromRoute] int collectionId, int movieId)
    {
      var item = await this.movieCollectionItems.GetById(movieId, collectionId);

      if (item == null)
        return NotFound(new { error = "Check collection id or movie id" });
        
      await this.movieCollectionItems.Delete(item);
      return NoContent();
    }
  }
}