using Moviet.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Models
{
    public class MovieVM
    {
        [Key]
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public TotalRatingVM Rating { get; set; }
        //public List<GenresVM> Genres { get; set; }
    }
}
