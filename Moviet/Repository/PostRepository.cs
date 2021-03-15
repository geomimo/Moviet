using Microsoft.EntityFrameworkCore;
using Moviet.Contracts;
using Moviet.Data;
using System.Collections.Generic;
using System.Linq;

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
            return IncludeAll().Where(p => !p.Movie.PostRemoved).ToList();
        }

        public List<Post> FindAllByUserId(string id)
        {
            return IncludeAll().FindAll(p => p.OwnerId == id);
        }

        private List<Post> IncludeAll()
        {
            return _db.Posts
                .Include(p => p.Owner)
                .Include(p => p.Movie)
                    .ThenInclude(m => m.Ratings)
                .Include(p => p.Movie)
                    .ThenInclude(m => m.Genres)
                        .ThenInclude(g => g.Genre)
                .ToList();
        }

        public List<Post> FindAllByGenreId(int id)
        {
            var posts = IncludeAll();
            List<Post> postsWithGenre = new List<Post>();
            foreach (var p in posts)
            {
                foreach (var mv in p.Movie.Genres)
                {
                    if (mv.GenreId == id)
                    {
                        postsWithGenre.Add(p);
                        break;
                    }
                }
            }
            return postsWithGenre;
        }

        public Post FindById(int id)
        {
            var post = IncludeAll().SingleOrDefault(p => p.PostId.Equals(id));
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

        

        public bool ExistsByMovieTitle(string title)
        {
            return IncludeAll().Exists(p => p.Movie.Title == title);
        }

        public void SetIdentityInsert(bool set)
        {
            if (set)
            {
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Posts] ON");
            }
            else
            {
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Posts] OFF");
            }
        }

        public void Clear()
        {
            _db.Database.ExecuteSqlRaw("TRUNCATE TABLE dbo.Posts");
            Save();
        }

        public List<Post> FindAllRatedByUserId(string id)
        {
            return IncludeAll().Where(p => p.Movie.Ratings.Any(r => r.Rater.Id == id)).ToList();
        }
    }
}
