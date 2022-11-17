using Microsoft.EntityFrameworkCore;

namespace lab2.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new LibraryDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<LibraryDbContext>>()
        );

        if (context.users!.Any())
        {
            return;
        }

        context.users!.AddRange(
            new User
            {
                username = "librarian",
                pwd = "123"
            },
            new User
            {
                username = "james",
                pwd = "123"
            },
            new User
            {
                username = "richard",
                pwd = "123"
            },
            new User
            {
                username = "user",
                pwd = "123"
            }
        );

        context.books!.AddRange(
            new Book
            {
                author = "Jeremy Clarkson",
                date = 2020,
                publisher = "Penguin Random House UK",
                title = "Can You Make This Thing Go Faster",
                reserved = null,
                leased = null
            },
            new Book
            {
                author = "Jeremy Clarkson",
                date = 2020,
                publisher = "Penguin Random House UK",
                title = "Diddly Squat - a Year on the Farm",
                reserved = null,
                leased = null
            },
            new Book
            {
                author = "Mario Puzo",
                date = 1969,
                publisher = "G. P. Putnam's Sons",
                title = "The Godfather",
                reserved = null,
                leased = null
            },
            new Book
            {
                author = "Arthur Conan Doyle",
                date = 1890,
                bookId = 10,
                publisher = "Lippincott's Monthly Magazine",
                title = "The Sigh of the Four",
                leased = null,
                reserved = null
            },
            new Book
            {
                author = "Charles Dickens",
                date = 1843,
                publisher = "Chapman & Hall",
                title = "A Christmas Carol",
                leased = null,
                reserved = null
            }
        );
        context.SaveChanges();
    }
}