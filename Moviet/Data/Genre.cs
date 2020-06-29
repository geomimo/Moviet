using System.ComponentModel.DataAnnotations;

namespace Moviet.Data
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }
        public string Name { get; set; }


    }
}
