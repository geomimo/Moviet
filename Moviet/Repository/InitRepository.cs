using Moviet.Contracts;
using Moviet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Repository
{
    public class InitRepository : IInitRepository
    {
        private readonly ApplicationDbContext _db;

        public InitRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        private bool SetInit()
        {
            _db.Init.Add(new Init() { IsInit = true });
            _db.SaveChanges();
            return false;
        }
        public bool IsInit()
        {
            return _db.Init.FirstOrDefault() == null ? SetInit() : true; //if no item, then not initialized.
        }
    }
}
