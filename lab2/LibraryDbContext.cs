using lab2.Models;
using Microsoft.EntityFrameworkCore;

namespace lab2;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext (DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book>? books { get; set; }

    public DbSet<User>? users { get; set; }
}