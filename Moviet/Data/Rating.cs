using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Data
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        public string RaterId { get; set; }
        public IdentityUser Rater { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public float Value { get; set; }
        public DateTime DateRated { get; set; }


    }
}
