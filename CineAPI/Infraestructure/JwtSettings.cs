namespace CineAPI.Infraestructure;

public class JwtSettings
{
    public string Key { get; set; } = default!;
    public int ExpirationMinutes { get; set; }
}
