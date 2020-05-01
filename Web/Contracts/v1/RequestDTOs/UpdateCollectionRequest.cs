namespace zMovies.Web.Contracts.V1.RequestDTOs
{
    public class UpdateCollectionRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}