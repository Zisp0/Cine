using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineAPI.Domain;

[Table("Movies")]
public class Movie
{
    [Key]
    public int MovieId { get; set; }
    public string Title { get; set; } = default!;
    public string? Director { get; set; }
    public string? Genre { get; set; }
    public DateTime? ReleaseDate { get; set; } 
    public int Duration { get; set; }
    public string? Description { get; set; }
    public string? OriginalLanguage { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
