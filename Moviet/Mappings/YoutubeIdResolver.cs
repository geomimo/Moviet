using AutoMapper;
using Moviet.Data;
using Moviet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Mappings
{
    public class Movie2EditMovieVMYoutubeIdResolver : IValueResolver<Movie, EditMovieVM, string>
    {
        public string Resolve(Movie source, EditMovieVM destination, string destMember, ResolutionContext context)
        {
            // From embed to original
            string id = "";
            if(source.YoutubeId != null)
            {
                id = "https://www.youtube.com/watch?v=" + source.YoutubeId.Split("/").Last();
            }
            return id;
        }
    }

    public class CreateMovieVM2MovieYoutubeIdResolver : IValueResolver<CreateMovieVM, Movie, string>
    {
        public string Resolve(CreateMovieVM source, Movie destination, string destMember, ResolutionContext context)
        {
            // From original to embed
            string id = "";
            if (source.YoutubeId != null)
            {
                id = "https://www.youtube.com/embed/" + source.YoutubeId.Split("v=").Last() + "?autoplay=1";
            }
            return id;
        }
    }

    public class EditMovieVM2MovieYoutubeIdResolver : IValueResolver<EditMovieVM, Movie, string>
    {
        public string Resolve(EditMovieVM source, Movie destination, string destMember, ResolutionContext context)
        {
            // From original to embed
            string id = "";
            if (source.YoutubeId != null)
            {
                id = "https://www.youtube.com/embed/" + source.YoutubeId.Split("v=").Last() + "?autoplay=1";
            }
            return id;
        }
    }
}