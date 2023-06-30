using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain;

namespace Domain.Entities;

[Table(name: "Cinemas", Schema = "dbo")]
public class Cinema : BaseEntity
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public virtual ICollection<MovieCinema>? MovieCinemas { get; set; }
}