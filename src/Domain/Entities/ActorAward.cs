using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table(name: "ActorAwards", Schema = "dbo")]
public class ActorAward
{
    public Guid ActorId { get; set; }
    public Guid AwardId { get; set; }
    public virtual Actor Actor { get; set; }
    public virtual Award Award { get; set; }
}