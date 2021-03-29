using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviet.Data
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }

        [ForeignKey("RaterId")]
        public string RaterId { get; set; }
        public ApplicationUser Rater { get; set; }


        [ForeignKey("MovieId")]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public float Value { get; set; }
        public DateTime DateRated { get; set; }


    }
}
