using Microsoft.EntityFrameworkCore;
using Moviet.Contracts;
using Moviet.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _db;

        public GenreRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Clear()
        {
            _db.Database.ExecuteSqlRaw("TRUNCATE TABLE dbo.Genres");
            Save();
        }

        public bool Create(Genre entity)
        {
            _db.Genres.Add(entity);
            return Save();
        }

        public bool Delete(Genre entity)
        {
            _db.Genres.Remove(entity);
            return Save();
        }

        public bool ExistsByName(string name)
        {
            return _db.Genres.ToList().Exists(g => g.Name == name);
        }

        public List<Genre> FindAll()
        {
            return _db.Genres.ToList();
        }

        public async Task<List<Genre>> FindAllAsync()
        {
            return _db.Genres.ToListAsync().Result;
        }

        public Genre FindById(int id)
        {
            return _db.Genres.Find(id);
        }

        public int GetIdByName(string name)
        {
            return _db.Genres.ToList().SingleOrDefault(g => g.Name == name).GenreId;
        }

        public bool Save()
        {
            bool res = _db.SaveChanges() > 0;
            return res;
        }

        public void SetIdentityInsert(bool set)
        {
            if (set)
            {
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Genres] ON");
            }
            else
            {
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Genres] OFF");
            }
        }

        public bool Update(Genre entity)
        {
            _db.Genres.Update(entity);
            return Save();
        }
    }
}
