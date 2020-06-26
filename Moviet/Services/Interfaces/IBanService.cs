using AutoMapper.Configuration.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Services.Interfaces
{
    public interface IBanService
    {
        public bool BanUser(string userId);
    }
}
