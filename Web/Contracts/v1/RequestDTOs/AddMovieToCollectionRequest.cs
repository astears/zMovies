namespace zMovies.Web.Contracts.V1.RequestDTOs
{
    public class AddMovieToCollectionRequest
    {
        public int Uid { get; set; }
        public int collectionId { get; set; }
        public int MovieId { get; set; }
    }
}