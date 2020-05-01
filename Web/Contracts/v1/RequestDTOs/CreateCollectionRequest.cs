namespace zMovies.Web.Contracts.V1.RequestDTOs
{
    public class CreateCollectionRequest
    {
        public int Uid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}