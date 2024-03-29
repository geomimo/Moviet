﻿using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Moviet.Contracts;
using Moviet.Data;
using System.Collections.Generic;
using System.Linq;

namespace Moviet.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _db;
        public MovieRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Clear()
        {
            _db.Database.ExecuteSqlRaw("TRUNCATE TABLE dbo.MovieGenres");
            _db.Database.ExecuteSqlRaw("TRUNCATE TABLE dbo.Movies");
            Save();
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

        public Movie FindById(int id)
        {
            Movie movies = _db.Movies.Find(id);
            return movies;
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }

        public void SetIdentityInsert(bool set)
        {
            if (set)
            {
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Movies] ON");
            }
            else
            {
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Movies] OFF");
            }
        }

        public bool Update(Movie entity)
        {
            _db.Movies.Update(entity);
            return Save();
        }

        public void InsertBulk(List<Movie> movies)
        {
            _db.Movies.BulkInsert(movies);
            _db.BulkSaveChanges();
        }
    }
}




