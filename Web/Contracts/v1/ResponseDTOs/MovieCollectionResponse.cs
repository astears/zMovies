using System.Collections.Generic;

namespace zMovies.Web.Contracts.V1.ResponseDTOs
{
    public class MovieCollectionResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<MovieCollectionItemResponse> Movies { get; set; } = new HashSet<MovieCollectionItemResponse>();
        public UserResponse user {get; set; }
    }
}