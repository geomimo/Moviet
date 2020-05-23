using Moviet.Contracts;
using Moviet.Data;
using System;
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

        public List<Genre> FindAll()
        {
            return _db.Genres.ToList();
        }

        public Genre FindById(int id)
        {
            return _db.Genres.Find(id);
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }

        public bool Update(Genre entity)
        {
            _db.Genres.Update(entity);
            return Save();
        }
    }
}
