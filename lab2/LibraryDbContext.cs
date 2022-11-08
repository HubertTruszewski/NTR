using lab2.Models;
using Microsoft.EntityFrameworkCore;

namespace lab2;

public class LibraryDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;port=3306;database=ntr;user=ntr;password=ntr", ServerVersion.AutoDetect(
            "server=localhost;port=3306;database=ntr;user=ntr;password=ntr"));
    }

    public DbSet<Book>? books { get; set; }

    public DbSet<User>? users { get; set; }
}