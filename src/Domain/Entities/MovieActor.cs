using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table(name: "MovieActors", Schema = "dbo")]
public class MovieActor
{
    public Guid MovieId { get; set; }
    public Guid ActorId { get; set; }
    public virtual Movie Movie { get; set; }
    public virtual Actor Actor { get; set; }
}