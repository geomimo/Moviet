using Moviet.Data;
using System.Collections.Generic;

namespace Moviet.Contracts
{
    public interface IPostRepository : IRepositoryBase<Post>
    {
        public List<Post> FindAllByUserId(string id);
        public List<Post> FindAllByGenreId(int id);
        public bool ExistsByMovieTitle(string title);
        public List<Post> FindAllRatedByUserId(string id);
    }
}
