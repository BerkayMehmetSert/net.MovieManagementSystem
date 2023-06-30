using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table(name: "MovieDirectors", Schema = "dbo")]
public class MovieDirector
{
    public Guid MovieId { get; set; }
    public Guid DirectorId { get; set; }
    public virtual Movie Movie { get; set; }
    public virtual Director Director { get; set; }
}