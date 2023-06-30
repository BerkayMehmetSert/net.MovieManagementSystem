using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain;

namespace Domain.Entities;

[Table(name: "Actors", Schema = "dbo")]
public class Actor : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Nationality { get; set; }
    public DateTime DateOfBirth { get; set; }
    public virtual ICollection<MovieActor>? MovieActors { get; set; }
    public virtual ICollection<ActorAward>? ActorAwards { get; set; }
}