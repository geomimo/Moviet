using Moviet.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
