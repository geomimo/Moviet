using Moviet.Data;
using System.Collections.Generic;

namespace Moviet.Contracts
{
    public interface IRatingRepository : IRepositoryBase<Rating>
    {
        public List<Rating> FindAllByUserId(string id);
    }
}
