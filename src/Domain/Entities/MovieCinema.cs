using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table(name: "MovieCinemas", Schema = "dbo")]
public class MovieCinema
{
    public Guid MovieId { get; set; }
    public Guid CinemaId { get; set; }
    public virtual Movie Movie { get; set; }
    public virtual Cinema Cinema { get; set; }
}