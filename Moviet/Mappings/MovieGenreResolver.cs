using AutoMapper;
using Moviet.Data;
using Moviet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Mappings
{
    public class CreateMovieGenreResolver : IValueResolver<CreateMovieVM, Movie, List<MovieGenre>>
    {
        public List<MovieGenre> Resolve(CreateMovieVM source, Movie destination, List<MovieGenre> destMember, ResolutionContext context)
        {
            List<MovieGenre> ls = new List<MovieGenre>();
            foreach(string id in source.Genres)
            {
                ls.Add(new MovieGenre { GenreId = Int32.Parse(id), MovieId = destination.MovieId });
            }

            return ls;
        }
    }

    public class EditMovieGenreResolver : IValueResolver<Movie, EditMovieVM, List<string>>
    {
        public List<string> Resolve(Movie source, EditMovieVM destination, List<string> destMember, ResolutionContext context)
        {
            List<string> ls = new List<string>();
            foreach(MovieGenre mv in source.Genres)
            {
                ls.Add(mv.Genre.Name);
            }

            return ls;
        }
    }
}
