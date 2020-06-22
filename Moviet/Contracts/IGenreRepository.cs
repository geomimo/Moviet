using Moviet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Contracts
{
    public interface IGenreRepository : IRepositoryBase<Genre>
    {
        bool ExistsByName(string name);
        public int GetIdByName(string name);
    }
}
