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
            CreateMap<Movie, CreateMovieVM>().ReverseMap()
                .ForMember(dest => dest.Genres, 
                           opt => opt.MapFrom<MovieGenreResolver>())
                .ForMember(dest => dest.Ratings, 
                           opt => opt.MapFrom<RatingResolver>());
            CreateMap<MovieVM, Movie>().ReverseMap()
                .ForMember(dest => dest.Rating,
                           opt => opt.MapFrom<TotalRatingResolver>());
            CreateMap<CreatePostVM, Post>().ReverseMap();
            CreateMap<MovieGenre, MovieGenreVM>().ReverseMap();
            CreateMap<Post, PostVM>().ReverseMap();
        }
    }
}
