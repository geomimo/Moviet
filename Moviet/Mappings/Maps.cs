using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moviet.Data;
using Moviet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<IdentityUser, IdentityUserVM>().ReverseMap();
            CreateMap<Genre, GenreVM>().ReverseMap();
            CreateMap<ContentManager, ContentManagerVM>().ReverseMap();
            CreateMap<MovieGenre, MovieGenreVM>().ReverseMap();

            // Movie Mappings
            CreateMap<Movie, CreateMovieVM>().ReverseMap()
                .ForMember(dest => dest.Genres,
                           opt => opt.MapFrom<CreateMovieGenreResolver>())
                .ForMember(dest => dest.Ratings, 
                           opt => opt.MapFrom<CreateRatingResolver>());

            CreateMap<Movie, EditMovieVM>()
                .ForMember(dest => dest.Genres,
                           opt => opt.MapFrom<EditMovieGenreMovie2EditMovieVMResolver>());

            CreateMap<EditMovieVM, Movie>()
                .ForMember(dest => dest.Genres,
                           opt => opt.MapFrom<EditMovieGenreEditMovieVM2MovieResolver>())
                .ForMember(dest => dest.Ratings,
                           opt => opt.MapFrom<EditRatingResolver>());

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
