using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table(name: "MovieGenres", Schema = "dbo")]
public class MovieGenre
{
    public Guid MovieId { get; set; }
    public Guid GenreId { get; set; }
    public virtual Movie Movie { get; set; }
    public virtual Genre Genre { get; set; }
}