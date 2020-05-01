using System.Collections.Generic;

namespace zMovies.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public byte[] Salt { get; set; }
        public byte[] Hash { get; set; }
        public ICollection<MovieRating> MovieRatings { get; set; }
        public ICollection<MovieCollection> MovieCollections { get; set; }
    }
}