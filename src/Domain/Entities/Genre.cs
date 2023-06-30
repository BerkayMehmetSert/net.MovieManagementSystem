using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain;

namespace Domain.Entities;

[Table(name: "Genres", Schema = "dbo")]
public class Genre : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual ICollection<MovieGenre>? MovieGenres { get; set; }
}