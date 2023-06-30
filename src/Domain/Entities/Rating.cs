using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain;

namespace Domain.Entities;

[Table(name: "Ratings", Schema = "dbo")]
public class Rating : BaseEntity
{
    public decimal Score { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<MovieRating>? MovieRatings { get; set; }
}