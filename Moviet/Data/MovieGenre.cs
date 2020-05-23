using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Data
{
    public class MovieGenre
    {
        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
