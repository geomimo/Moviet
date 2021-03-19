using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Services.Interfaces
{
    public interface IPornDetectorService
    {
        public bool IsPorn(string image_path, string text);
    }
}
