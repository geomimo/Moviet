using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Data
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string OwnerId { get; set; }
        public IdentityUser Owner { get; set; }
        public DateTime DateCreated { get; set; }
        public Movie Movie { get; set; }
    }
}
