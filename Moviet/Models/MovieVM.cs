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
        [Required]
        public string Title { get; set; }

        [Display(Name ="Sort Description")]
        [Range(10,50)]
        [Required]
        public string SortDescription { get; set; }

        [Display(Name = "Long Description")]
        [Range(100, 850)]
        [Required]
        public string LongDescription { get; set; }

        [Required]
        public Rating Rating { get; set; }
        public string PosterPath { get; set; }

        [FileExtensions(Extensions = "jpg,gif,png")]
        [Required]
        public IFormFile Poster { get; set; }

        [Required]
        [Display(Name = "Trailer")]
        //[RegularExpression("(https:\/\/| http:\/\/)?(www.)? youtube.com\/watch\?v=[a - zA - Z0 - 9_] *)")]
        public string YoutubeId { get; set; }

        [Display(Name = "Genres")]
        public List<SelectListItem> AvailableGenres { get; set; }

        [Required]
        public List<int> Genres { get; set; }
    }

    public class EditMovieVM
    {
        public int MovieId { get; set; }

        [Required]
        public string Title { get; set; }

        [Display(Name = "Sort Description")]
        [Range(10, 50)]
        [Required]
        public string SortDescription { get; set; }

        [Display(Name = "Long Description")]
        [Range(100, 850)]
        [Required]
        public string LongDescription { get; set; }

        [Required]
        public Rating Rating { get; set; }
        public string PosterPath { get; set; }

        [FileExtensions(Extensions = "jpg,gif,png")]
        [Required]
        public IFormFile Poster { get; set; }

        [Display(Name = "Trailer")]
        //[RegularExpression("(https:\/\/| http:\/\/)?(www.)? youtube.com\/watch\?v=[a - zA - Z0 - 9_] *)")]
        [Required]
        public string YoutubeId { get; set; }

        [Display(Name = "Genres")]
        public List<SelectListItem> AvailableGenres { get; set; }

        [Required]
        public List<int> Genres { get; set; }
    }

}
