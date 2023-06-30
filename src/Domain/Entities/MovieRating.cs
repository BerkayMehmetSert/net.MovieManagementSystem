using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table(name: "MovieRatings", Schema = "dbo")]
public class MovieRating
{
    public Guid MovieId { get; set; }
    public Guid RatingId { get; set; }
    public virtual Movie Movie { get; set; }
    public virtual Rating Rating { get; set; }
}