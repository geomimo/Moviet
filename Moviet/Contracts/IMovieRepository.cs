using Moviet.Data;
using System.Collections.Generic;

namespace Moviet.Contracts
{
    public interface IMovieRepository : IRepositoryBase<Movie>
    {
        public void InsertBulk(List<Movie> movies);

    }
}

