using Moviet.Contracts;
using Moviet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _db;
        public MovieRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Create(Movie entity)
        {
            _db.Movies.Add(entity);
            return Save();
        }

        public bool Delete(Movie entity)
        {
            _db.Movies.Remove(entity);
            return Save();
        }

        public List<Movie> FindAll()
        {
            var movies = _db.Movies.ToList();
            return movies;
        }

        public Movie FindById( int id)
        {
           Movie movies = _db.Movies.Find(id);
           return movies;
        } 

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }

        public bool Update(Movie entity)
        {
            _db.Movies.Update(entity);
            return Save();
        }
    }
}

    
    

