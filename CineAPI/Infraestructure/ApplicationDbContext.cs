using CineAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.Infraestructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Movie> Movies { get; set; }
}