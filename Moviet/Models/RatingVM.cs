using System;
using System.ComponentModel.DataAnnotations;

namespace Moviet.Models
{
    public class RatingVM
    {
        public int RatingId { get; set; }
        public IdentityUserVM Rater { get; set; }
        public MovieVM Movie { get; set; }
        public float Value { get; set; }

        [Display(Name = "Date Rated")]
        public DateTime DateRated { get; set; }
    }
}
