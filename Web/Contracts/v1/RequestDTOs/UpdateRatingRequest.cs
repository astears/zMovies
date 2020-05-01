namespace zMovies.Web.Contracts.V1.RequestDTOs
{
    public class UpdateRatingRequest
    {
        public int Uid { get; set; }
        public int Value { get; set; }
        public string Review { get; set; }
        public int MovieId { get; set; } 
    }
}