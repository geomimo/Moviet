using System.ComponentModel.DataAnnotations;

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
