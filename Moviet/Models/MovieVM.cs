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
       
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string SortDescription { get; set; }
        public string LongDescription { get; set; }
        public float Rating { get; set; }

        [Display(Name = "Poster")]
        public string PosterPath { get; set; }
        [Display(Name = "Trailer")]
        public string YoutubeId { get; set; }
        public List<MovieGenreVM> Genres { get; set; }
    }

    public class CreateMovieVM
    {
        public string Title { get; set; }

        [Display(Name ="Sort Description")]
        public string SortDescription { get; set; }

        [Display(Name = "Long Description")]
        public string LongDescription { get; set; }
        public float Rating { get; set; }
        public string PosterPath { get; set; }
        public IFormFile Poster { get; set; }
        [Display(Name = "Trailer")]
        public string YoutubeId { get; set; }

        [Display(Name = "Genres")]
        public List<SelectListItem> AvailableGenres { get; set; }
        public List<string> Genres { get; set; }
    }

    public class EditMovieVM
    {
        public int MovieId { get; set; }

        public string Title { get; set; }

        [Display(Name = "Sort Description")]
        public string SortDescription { get; set; }

        [Display(Name = "Long Description")]
        public string LongDescription { get; set; }
        public Rating OwnersRating { get; set; }
        public string PosterPath { get; set; }
        public IFormFile Poster { get; set; }
        [Display(Name = "Trailer")]
        public string YoutubeId { get; set; }

        [Display(Name = "Genres")]
        public List<SelectListItem> AvailableGenres { get; set; }
        public List<MovieGenreVM> Genres { get; set; }
    }

}
