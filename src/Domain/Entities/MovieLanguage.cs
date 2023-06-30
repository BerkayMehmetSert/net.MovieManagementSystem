using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table(name: "MovieLanguages", Schema = "dbo")]
public class MovieLanguage
{
    public Guid MovieId { get; set; }
    public Guid LanguageId { get; set; }
    public virtual Movie Movie { get; set; }
    public virtual Language Language { get; set; }
}