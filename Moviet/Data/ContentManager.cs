using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Data
{
    public class ContentManager : Rater
    {
        public Registrator Owner { get; set; }
        public DateTime DateCreated { get; set; }
        //public List<Post> OwnPosts { get; set; }
    }
}
