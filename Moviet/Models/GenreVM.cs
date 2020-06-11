using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Models
{
    public class GenreVM
    {
        public int GenreId { get; set; }

        [MinLength(3)]
        [MaxLength(25)]
        [Required]
        public string Name { get; set; }
    }
}
