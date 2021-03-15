using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviet.Data
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        public DateTime DateCreated { get; set; }
    
        [Required]
        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }
    }
}
