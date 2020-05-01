using System.Collections.Generic;

namespace zMovies.Core.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public ICollection<MovieRating> MovieRatings { get; set; }
    }
}