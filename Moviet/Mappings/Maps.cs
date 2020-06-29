using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moviet.Data;
using Moviet.Models;

namespace Moviet.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<IdentityUser, IdentityUserVM>().ReverseMap();
            CreateMap<Genre, GenreVM>().ReverseMap();
            CreateMap<MovieGenre, MovieGenreVM>().ReverseMap();

            // Movie Mappings
            CreateMap<CreateMovieVM, Movie>()
                .ForMember(dest => dest.Genres,
                           opt => opt.MapFrom<CreateMovieGenreResolver>())
                .ForMember(dest => dest.Ratings,
                           opt => opt.MapFrom<CreateRatingResolver>())
                .ForMember(dest => dest.YoutubeId,
                           opt => opt.MapFrom<CreateMovieVM2MovieYoutubeIdResolver>());

            CreateMap<Movie, EditMovieVM>()
                .ForMember(dest => dest.Genres,
                           opt => opt.MapFrom<EditMovieGenreMovie2EditMovieVMResolver>())
                .ForMember(dest => dest.YoutubeId,
                           opt => opt.MapFrom<Movie2EditMovieVMYoutubeIdResolver>());

            CreateMap<EditMovieVM, Movie>()
                .ForMember(dest => dest.Genres,
                           opt => opt.MapFrom<EditMovieGenreEditMovieVM2MovieResolver>())
                .ForMember(dest => dest.Ratings,
                           opt => opt.MapFrom<EditRatingResolver>())
                .ForMember(dest => dest.YoutubeId,
                           opt => opt.MapFrom<EditMovieVM2MovieYoutubeIdResolver>());

            CreateMap<MovieVM, Movie>().ReverseMap()
                .ForMember(dest => dest.Rating,
                           opt => opt.MapFrom<TotalRatingResolver>());

            // Post Mappings
            CreateMap<Post, CreatePostVM>().ReverseMap();
            CreateMap<Post, PostVM>().ReverseMap();
            CreateMap<Post, EditPostVM>().ReverseMap();

            // Rating Mappings
            CreateMap<Rating, RatingVM>().ReverseMap();
        }
    }
}
