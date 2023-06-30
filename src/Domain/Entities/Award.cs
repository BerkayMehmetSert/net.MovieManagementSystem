using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain;

namespace Domain.Entities;

[Table(name: "Awards", Schema = "dbo")]
public class Award : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public virtual ICollection<ActorAward>? ActorAwards { get; set; }
}