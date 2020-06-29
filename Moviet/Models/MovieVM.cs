using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moviet.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Moviet.Models
{
    public class MovieVM
    {

        public int MovieId { get; set; }
        public string Title { get; set; }
        public string SortDescription { get; set; }
        public string LongDescription { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public float Rating { get; set; }

        public Rating UserRating { get; set; }

        [Display(Name = "Poster")]
        public string PosterPath { get; set; }
        [Display(Name = "Trailer")]
        public string YoutubeId { get; set; }
        public List<MovieGenreVM> Genres { get; set; }
        public bool PostRemoved { get; set; }

    }

    public class CreateMovieVM
    {
        [Required]
        public string Title { get; set; }

        [Display(Name = "Sort Description")]
        [MinLength(10)]
        [MaxLength(150)]
        [Required]
        public string SortDescription { get; set; }

        [Display(Name = "Long Description")]
        [MinLength(100)]
        [MaxLength(850)]
        [Required]
        public string LongDescription { get; set; }

        public Rating Rating { get; set; }
        public string PosterPath { get; set; }

        [FileExtensions(Extensions = "jpg,gif,png")]
        [Required]
        public IFormFile Poster { get; set; }

        [Required]
        [Display(Name = "Trailer")]
        [RegularExpression(@"^(https:\/\/|http:\/\/)?(www.)?youtube.com\/watch\?v=[a-zA-Z0-9_\-]*$")]
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
        [MinLength(10)]
        [MaxLength(150)]
        [Required]
        public string SortDescription { get; set; }

        [Display(Name = "Long Description")]
        [MinLength(100)]
        [MaxLength(850)]
        [Required]
        public string LongDescription { get; set; }
        public Rating Rating { get; set; }
        public string PosterPath { get; set; }

        [FileExtensions(Extensions = "jpg,gif,png")]
        public IFormFile Poster { get; set; }

        [Display(Name = "Trailer")]
        [RegularExpression(@"^(https:\/\/|http:\/\/)?(www.)?youtube.com\/watch\?v=[a-zA-Z0-9_\-]*$")]
        [Required]
        public string YoutubeId { get; set; }

        [Display(Name = "Genres")]
        public List<SelectListItem> AvailableGenres { get; set; }

        [Required]
        public List<int> Genres { get; set; }
    }

}
