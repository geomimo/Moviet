using AutoMapper;
using Moviet.Data;
using Moviet.Models;
using System.Collections.Generic;

namespace Moviet.Mappings
{
    public class CreateMovieGenreResolver : IValueResolver<CreateMovieVM, Movie, List<MovieGenre>>
    {
        public List<MovieGenre> Resolve(CreateMovieVM source, Movie destination, List<MovieGenre> destMember, ResolutionContext context)
        {
            List<MovieGenre> ls = new List<MovieGenre>();
            foreach (int id in source.Genres)
            {
                ls.Add(new MovieGenre { GenreId = id, MovieId = destination.MovieId });
            }

            return ls;
        }
    }

    public class EditMovieGenreMovie2EditMovieVMResolver : IValueResolver<Movie, EditMovieVM, List<int>>
    {
        public List<int> Resolve(Movie source, EditMovieVM destination, List<int> destMember, ResolutionContext context)
        {
            List<int> ls = new List<int>();
            foreach (MovieGenre mv in source.Genres)
            {
                ls.Add(mv.Genre.GenreId);
            }

            return ls;
        }
    }

    public class EditMovieGenreEditMovieVM2MovieResolver : IValueResolver<EditMovieVM, Movie, List<MovieGenre>>
    {
        public List<MovieGenre> Resolve(EditMovieVM source, Movie destination, List<MovieGenre> destMember, ResolutionContext context)
        {
            List<MovieGenre> ls = new List<MovieGenre>();
            foreach (int gid in source.Genres)
            {
                ls.Add(new MovieGenre { GenreId = gid, MovieId = source.MovieId });
            }

            return ls;
        }
    }



}
