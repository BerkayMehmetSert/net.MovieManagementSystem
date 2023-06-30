using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain;

namespace Domain.Entities;

[Table(name: "Directors", Schema = "dbo")]
public class Director : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Nationality { get; set; }
    public DateTime DateOfBirth { get; set; }
    public virtual ICollection<MovieDirector>? MovieDirectors { get; set; }
}