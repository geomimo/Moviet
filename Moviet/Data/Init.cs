using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Data
{
    public class Init
    {
        [Key]
        public int Id { get; set; }
        public bool IsInit { get; set; }
    }
}
