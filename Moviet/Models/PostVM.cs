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
        public ContentManager Owner { get; set; }

        [Display(Name = "Date Created:")]
        public DateTime DateCreated { get; set; }
        public Movie movie { get; set; }
    }
}
