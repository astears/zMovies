namespace zMovies.Core.Entities
{
    public class MovieRating
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int RatingId { get; set; }
        public Rating Rating { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Review { get; set; }
    }
}