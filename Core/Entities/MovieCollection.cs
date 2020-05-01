using System.Collections.Generic;

namespace zMovies.Core.Entities
{
    public class MovieCollection
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<MovieCollectionItem> MovieCollectionItems { get; set; } = new HashSet<MovieCollectionItem>();
        public User User { get; set; }
    }
}