using Moviet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Moviet.Contracts
{
    public interface IGenreRepository : IRepositoryBase<Genre>
    {
        public Task<List<Genre>> FindAllAsync();
        public bool ExistsByName(string name);
        public int GetIdByName(string name);
    }
}
