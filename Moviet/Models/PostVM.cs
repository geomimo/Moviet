﻿using Moviet.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Models
{
    public class PostVM
    {
        public ContentManagerVM Owner { get; set; }

        [Display(Name = "Date Created:")]
        public DateTime DateCreated { get; set; }
        public MovieVM Movie { get; set; }
    }
    public class CreatePostVM
    {
        public ContentManagerVM Owner { get; set; }

        [Display(Name = "Date Created:")]
        public DateTime DateCreated { get; set; }
        public CreateMovieVM Movie { get; set; }
    }

}
