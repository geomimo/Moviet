using Moviet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Contracts
{
    public interface IMovieGenreRepository : IRepositoryBase<MovieGenre>
    {
        public void InsertBulk(List<MovieGenre> movieGenres);

    }
}
