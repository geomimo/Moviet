using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviet.Data
{
    public class Movie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string SortDescription { get; set; }
        public string LongDescription { get; set; }
        public List<Rating> Ratings { get; set; }
        public string PosterPath { get; set; }
        public string YoutubeId { get; set; }
        public List<MovieGenre> Genres { get; set; }
        public bool PostRemoved { get; set; }
    }
}
