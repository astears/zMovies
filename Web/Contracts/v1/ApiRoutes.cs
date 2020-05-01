namespace zMovies.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Collections {
            public const string GetCollectionById = Base + "/collections/{collectionId}";
            public const string GetAllCollectionsByUser = Base + "/collections/user/{uid}";
            public const string CreateCollection = Base + "/collections";
            public const string UpdateCollection = Base + "/collections/{collectionId}";
            public const string DeleteCollection = Base + "/collections/{collectionId}";
            public const string AddMovieToCollection = Base + "/collections/{collectionId}/movie/{movieId}";
            public const string DeleteMovieFromCollection = Base + "/collections/{collectionId}/movie/{movieId}";

        }

        public static class Ratings {
            public const string GetAllRatingsByUser = Base + "/ratings/user/{uid}";
            public const string GetAllRatingsByMovie = Base + "/ratings/movie/{movieId}";
            public const string Create = Base + "/ratings";
            public const string Update = Base + "/ratings";
            public const string Delete = Base + "/ratings/{uid}/{movieId}";
        }
        
    }
}