using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Services.Interfaces
{
    public interface IPosterUploadService 
    {
        public string UploadImage(IFormFile file);
    }
}
