using Microsoft.EntityFrameworkCore;
using Moviet.Contracts;
using Moviet.Data;
using System.Collections.Generic;
using System.Linq;

namespace Moviet.Repository
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext _db;

        public RatingRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Create(Rating entity)
        {
            _db.Ratings.Add(entity);
            return Save();
        }

        public bool Delete(Rating entity)
        {
            _db.Ratings.Remove(entity);
            return Save();
        }

        public List<Rating> FindAll()
        {
            return IncludeAll();

        }

        public Rating FindById(int id)
        {
            return IncludeAll().Find(r => r.RatingId == id);

        }

        public List<Rating> FindAllByUserId(string id)
        {
            return IncludeAll().Where(r => r.Rater.Id == id).ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }

        public bool Update(Rating entity)
        {
            _db.Ratings.Update(entity);
            return Save();
        }

        private List<Rating> IncludeAll()
        {
            return _db.Ratings.Include(r => r.Rater)
                              .Include(r => r.Movie)
                              .ToList();
        }


    }
}
