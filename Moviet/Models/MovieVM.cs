using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public TotalRatingVM Rating { get; set; }
        public string PosterPath { get; set; }
        [Display(Name = "Trailer")]
        public string YoutubeId { get; set; }
        public List<ListMovieGenreVM> Genres { get; set; }
    }

    public class CreateMovieVM
    {
        [Key]
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        public string PosterPath { get; set; }
        public IFormFile Poster { get; set; }
        [Display(Name = "Trailer")]
        public string YoutubeId { get; set; }

        [Display(Name = "Genres")]
        public List<SelectListItem> AvailableGenres { get; set; }

        public List<string> Genres { get; set; }
    }
}
