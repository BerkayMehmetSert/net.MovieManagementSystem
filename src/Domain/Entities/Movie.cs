using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain;

namespace Domain.Entities;

[Table(name:"Movies", Schema = "dbo")]
public class Movie : BaseEntity
{
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Plot { get; set; }
    public int MovieLength { get; set; }
    public virtual ICollection<MovieActor>? MovieActors { get; set; }
    public virtual ICollection<MovieDirector>? MovieDirectors { get; set; }
    public virtual ICollection<MovieCinema>? MovieCinemas { get; set; }
    public virtual ICollection<MovieRating>? MovieRatings { get; set; }
    public virtual ICollection<MovieGenre>? MovieGenres { get; set; }
    public virtual ICollection<MovieLanguage>? MovieLanguages { get; set; }
}