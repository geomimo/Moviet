namespace Moviet.Models
{
    public class MovieGenreVM
    {
        public int GenreId { get; set; }
        public GenreVM Genre { get; set; }

        public int MovieId { get; set; }
        public MovieVM Movie { get; set; }
    }
}
