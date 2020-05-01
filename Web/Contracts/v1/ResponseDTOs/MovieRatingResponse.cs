namespace zMovies.Web.Contracts.V1.ResponseDTOs
{
    public class MovieRatingResponse
    {
        public int MovieId { get; set; }

        public RatingResponse Rating { get; set; }

        public UserResponse User { get; set; }

        public string Review { get; set; }
    }
}