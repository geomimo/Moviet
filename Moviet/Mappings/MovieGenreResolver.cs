using AutoMapper;
using Moviet.Data;
using Moviet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Mappings
{
    public class MovieGenreResolver : IValueResolver<CreateMovieVM, Movie, List<MovieGenre>>
    {
        public List<MovieGenre> Resolve(CreateMovieVM source, Movie destination, List<MovieGenre> destMember, ResolutionContext context)
        {
            List<MovieGenre> ls = new List<MovieGenre>();
            foreach(string v in source.Genres)
            {
                Genre g = new Genre { Name = v };
                ls.Add(new MovieGenre { Genre = g, Movie = destination });
            }

            return ls;
        }
    }
}
