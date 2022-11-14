using lab3.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext (DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book>? books { get; set; }

    public DbSet<User>? users { get; set; }
}