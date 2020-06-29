using Microsoft.AspNetCore.Http;

namespace Moviet.Services.Interfaces
{
    public interface IPosterUploadService
    {
        public string UploadImage(IFormFile file);
    }
}
