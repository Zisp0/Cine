namespace CineAPI.Common.Dtos;

public class MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Director { get; set; } = default!;
    public string? Genre { get; set; } = default!;
    public DateTime? ReleaseDate { get; set; }
    public int Duration { get; set; }
    public string? Description { get; set; } = default!;
    public string? OriginalLanguage { get; set; } = default!;
}
