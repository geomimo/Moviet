using Microsoft.AspNetCore.Identity;
using Moviet.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Models
{
    public class PostVM
    {
        public int PostID { get; set; }
        public IdentityUserVM Owner { get; set; }

        [Display(Name = "Date Created")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
        public MovieVM Movie { get; set; }
    }
    public class CreatePostVM
    {
        public IdentityUserVM Owner { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        public CreateMovieVM Movie { get; set; }
    }

    public class EditPostVM
    {
        public int PostID { get; set; }
        public IdentityUserVM Owner { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        public EditMovieVM Movie { get; set; }
    }

}
