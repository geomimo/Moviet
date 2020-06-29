using System;
using System.ComponentModel.DataAnnotations;

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
