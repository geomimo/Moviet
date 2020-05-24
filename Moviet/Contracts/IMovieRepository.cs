using Moviet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Contracts
{
    public interface IMovieRepository : IRepositoryBase<Movie>
    {
        object Find(int id);
        object FindAll(int id);
    }
}

