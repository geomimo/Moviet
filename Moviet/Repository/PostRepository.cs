using Microsoft.EntityFrameworkCore;
using Moviet.Contracts;
using Moviet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _db;
        public PostRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool Create(Post entity)
        {
            _db.Posts.Add(entity);
            return Save();
        }

        public bool Delete(Post entity)
        {
            _db.Posts.Remove(entity);
            return Save();
        }

        public List<Post> FindAll()
        {
            return _db.Posts.Include(p => p.Owner)
                                .Include(p => p.Movie)
                                    .ThenInclude(m => m.Ratings)
                                .Include(p => p.Movie)
                                    .ThenInclude(m => m.Genres)
                                .ToList();
        }

        public Post FindById(int id)
        {
            var post = _db.Posts.Include(p => p.Owner)
                                .Include(p => p.Movie)
                                    .ThenInclude(m => m.Ratings)
                                .Include(p => p.Movie)
                                    .ThenInclude(m => m.Genres)
                                .SingleOrDefault(p => p.PostId.Equals(id));
            return post;

        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }

        public bool Update(Post entity)
        {
            _db.Posts.Update(entity);
            return Save();
        }
    }
}
