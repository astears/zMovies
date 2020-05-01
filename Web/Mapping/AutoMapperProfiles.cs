using AutoMapper;
using zMovies.Core.Entities;
using zMovies.Web.Contracts.V1.ResponseDTOs;

namespace zMovies.Web.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() {
            CreateMap<User, UserResponse>();
            CreateMap<MovieCollectionItem, MovieCollectionItemResponse>();
            CreateMap<Rating, RatingResponse>();
            CreateMap<MovieRating, MovieRatingResponse>();
            CreateMap<MovieCollection, MovieCollectionResponse>()
                .ForMember(dest => dest.Movies, opt => opt.MapFrom(src => src.MovieCollectionItems));
        }
    }
}