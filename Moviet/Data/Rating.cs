using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Data
{
    public class Rating
    {
        public int RatingId { get; set; }

        public Rater Rater { get; set; }

        public float Value { get; set; }

        public DateTime DateRated { get; set; }


    }
}
