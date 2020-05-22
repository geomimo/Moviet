using Microsoft.AspNetCore.Identity;
using Moviet.Contracts;
using Moviet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public bool Create(IdentityUser entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(IdentityUser entity)
        {
            throw new NotImplementedException();
        }

        public List<IdentityUser> FindAll()
        {
            throw new NotImplementedException();
        }

        public IdentityUser FindById()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool Update(IdentityUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
