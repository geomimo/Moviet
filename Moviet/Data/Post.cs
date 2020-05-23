using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Data
{
    public class Post
    {
        public ContentManager Owner { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
