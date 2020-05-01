namespace zMovies.Core.Entities
{
    public class MovieCollectionItem
    {
        public int MovieId { get; set; }
        public int MovieCollectionId { get; set; }
        public MovieCollection MovieCollection { get; set; }
    }
}