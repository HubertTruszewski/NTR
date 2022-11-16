using lab3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab3.Controllers;

public class BookController : Controller
{
    private readonly LibraryDbContext _context;

    public BookController(LibraryDbContext context)
    {
        _context = context;
    }

    [Authorize]
    public IActionResult List()
    {
        using var context = _context;
        var books = context.books!.AsNoTracking().ToList();
        ViewBag.Message = TempData["message"]!;
        ViewBag.ConcurrencyError = TempData["concurrencyError"]!;
        return View(books);
    }

    [HttpPost]
    public IActionResult List(string phrase)
    {
        List<Book> books;
        using var context = _context;
        if (string.IsNullOrEmpty(phrase))
        {
            books = context.books!.ToList();
            return View(books);
        }

        books = context.books!.AsNoTracking().Where(b => b.title!.ToLower().Contains(phrase.ToLower())).ToList();
        ViewBag.Message = TempData["message"]!;
        if (!books.Any())
        {
            ViewBag.Message = "Not found books with title containing: " + phrase;
        }

        ViewBag.Phrase = phrase;
        return View(books);
    }

    [Authorize]
    public IActionResult Reservations()
    {
        using var context = _context;
        var reservedBooks = context.books!.AsNoTracking().Include("user").AsEnumerable().Where(b => b.IsReserved()).ToList();
        ViewBag.Message = TempData["message"]!;
        ViewBag.ConcurrencyError = TempData["concurrencyError"]!;
        return View(reservedBooks);
    }

    [Authorize]
    public IActionResult Borrowings()
    {
        using var context = _context;
        var borrowedBooks = context.books!.AsNoTracking().Include("user").AsEnumerable().Where(b => b.IsLeased()).ToList();
        ViewBag.Message = TempData["message"]!;
        ViewBag.ConcurrencyError = TempData["concurrencyError"]!;
        return View(borrowedBooks);
    }

    [Authorize]
    [HttpPost]
    public IActionResult ReserveBook(BookActionModel bookAction)
    {
        using var context = _context;
        var book = context.books!.First(b => b.bookId == bookAction.book);
        var user = context.users!.First(u => u.username == bookAction.user);
        _context.Entry(book).Property("rowVersion").OriginalValue = bookAction.rowVersion;
        book.Reserve(user);
        try
        {
            context.SaveChanges();
            TempData["message"] = "Success! You reserved a book: " + book.title;
        }
        catch (DbUpdateException)
        {
            TempData["concurrencyError"] =
                "The book's record you try to reserve was changed by another user. Try again.";
        }

        return RedirectToAction("List");
    }

    [Authorize]
    [HttpPost]
    public IActionResult CancelReservation(BookActionModel bookAction)
    {
        using var context = _context;
        var book = context.books!.Include("user").First(b => b.bookId == bookAction.book);
        _context.Entry(book).Property("rowVersion").OriginalValue = bookAction.rowVersion;
        book.CancelReservation();
        try
        {
            context.SaveChanges();
            TempData["message"] = "You canceled reservation for book: " + book.title;
        }
        catch (DbUpdateException)
        {
            TempData["concurrencyError"] = "The book's record you try to modify was changed by another user. Try again.";
        }
        
        return RedirectToAction("Reservations");
    }

    [Authorize]
    [HttpPost]
    public IActionResult BorrowBook(BookActionModel bookAction)
    {
        using var context = _context;
        var book = context.books!.First(b => b.bookId == bookAction.book);
        book.Lease();
        _context.Entry(book).Property("rowVersion").OriginalValue = bookAction.rowVersion;
        try
        {
            context.SaveChanges();
            TempData["message"] = "Success! Book " + book.title + " borrowed";
        }
        catch (DbUpdateException)
        {
            TempData["concurrencyError"] = "The book's record you try to modify was changed by another user. Try again.";
        }
        
        return RedirectToAction("Reservations");
    }

    [Authorize]
    [HttpPost]
    public IActionResult ReturnBook(BookActionModel bookAction)
    {
        using var context = _context;
        var book = context.books!.Include("user").First(b => b.bookId == bookAction.book);
        book.Return();
        _context.Entry(book).Property("rowVersion").OriginalValue = bookAction.rowVersion;
        try
        {
            context.SaveChanges();
            TempData["message"] = "Success! Book " + book.title + " returned";
        }
        catch (DbUpdateException)
        {
            TempData["concurrencyError"] = "The book's record you try to modify was changed by another user. Try again.";
        }

        return RedirectToAction("Borrowings");
    }
}