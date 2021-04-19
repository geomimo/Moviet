using Moviet.Contracts;
using Moviet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Repository
{
    public class MovieGenreRepository : IMovieGenreRepository
    {
        private readonly ApplicationDbContext _db;

        public MovieGenreRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Create(MovieGenre entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(MovieGenre entity)
        {
            throw new NotImplementedException();
        }

        public List<MovieGenre> FindAll()
        {
            throw new NotImplementedException();
        }

        public MovieGenre FindById(int id)
        {
            throw new NotImplementedException();
        }

        public void InsertBulk(List<MovieGenre> movieGenres)
        {
            _db.MovieGenres.BulkInsert(movieGenres);
            _db.BulkSaveChanges();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void SetIdentityInsert(bool set)
        {
            throw new NotImplementedException();
        }

        public bool Update(MovieGenre entity)
        {
            throw new NotImplementedException();
        }


    }
}
