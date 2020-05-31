using Moviet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Contracts
{
    public interface IRatingRepository : IRepositoryBase<Rating>
    {
        public List<Rating> FindAllByUserId(string id);
    }
}
