using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain;

namespace Domain.Entities;

[Table(name: "Languages", Schema = "dbo")]
public class Language : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual ICollection<MovieLanguage>? MovieLanguages { get; set; }
}