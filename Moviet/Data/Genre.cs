using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviet.Data
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }
        public string Name { get; set; }


    }
}
