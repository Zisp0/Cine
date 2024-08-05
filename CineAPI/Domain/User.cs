using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineAPI.Domain;

[Table("Users")]
public class User
{ 
    [Key]
    public int UserId { get; set; }
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public DateTime RegisteredDate { get; set; }

}
