using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Models
{
    public class RatingVM
    {
        public int RatingId { get; set; }
        public float Value { get; set; }
        public int MovieId { get; set; }
        public DateTime DateRated { get; set; }
    }
}
