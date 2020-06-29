using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

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
