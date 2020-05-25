using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moviet.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Moviet.Services
{
    public class PosterUploadService : IPosterUploadService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PosterUploadService(IWebHostEnvironment hostingEnviroment)
        {
            _hostingEnvironment = hostingEnviroment;
        }   

        public string UploadImage(IFormFile file)
        {
            string toFolder = Path.Combine(_hostingEnvironment.WebRootPath, "posters");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(toFolder, uniqueFileName);
            file.CopyTo(new FileStream(filePath, FileMode.Create));
            return Path.Combine("img", uniqueFileName);
        }
    }
}
