using Moviet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Contracts
{
    public interface IGenreRepository : IRepositoryBase<Genre>
    {
        public Task<List<Genre>> FindAllAsync();
    }
}
