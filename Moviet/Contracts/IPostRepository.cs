using Moviet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Contracts
{
    public interface IPostRepository : IRepositoryBase<Post>
    {
        public List<Post> FindAllByUserId(string id);
        public List<Post> FindAllByGenreId(int id);

    }
}
