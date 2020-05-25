using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Models
{
    public class MovieGenreVM
    {
        public int GenreId { get; set; }
        public GenreVM Genre { get; set; }

        public int MovieId { get; set; }
        public MovieVM Movie { get; set; }
    }
}
