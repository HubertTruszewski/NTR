using System.Linq.Expressions;
using System.Text.Json;
using lab2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab2.Controllers;

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
        var books = context.books!.ToList();
        ViewBag.Message = TempData["message"]!;
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
        
        books = context.books!.Where(b => b.title!.ToLower().Contains(phrase.ToLower())).ToList();
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
        var reservedBooks = context.books!.Include("user").ToList().Where(b => b.IsReserved()).ToList();
        ViewBag.Message = TempData["message"]!;
        return View(reservedBooks);
    }
    
    [Authorize]
    public IActionResult Borrowings()
    {
        using var context = _context;
        var borrowedBooks = context.books!.Include("user").ToList().Where(b => b.IsLeased()).ToList();
        ViewBag.Message = TempData["message"]!;
        return View(borrowedBooks);
    }

    [Authorize]
    [HttpPost]
    public IActionResult ReserveBook(BookActionModel bookAction)
    {
        using var context = _context;
        var book = context.books!.First(b => b.bookId == bookAction.book);
        var user = context.users!.First(u => u.username == bookAction.user);
        book.Reserve(user);
        context.SaveChanges();
        TempData["message"] = "Success! You reserved a book: " + book.title;
        return RedirectToAction("List");
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult CancelReservation(BookActionModel bookAction)
    {
        using var context = _context;
        var book = context.books!.First(b => b.bookId == bookAction.book);
        book.CancelReservation();
        context.SaveChanges();
        TempData["message"] = "You canceled reservation for book: " + book.title;
        return RedirectToAction("Reservations");
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult BorrowBook(BookActionModel bookAction)
    {
        using var context = _context;
        var book = context.books!.First(b => b.bookId == bookAction.book);
        book.Lease();
        context.SaveChanges();
        TempData["message"] = "Success! Book " + book.title + " borrowed";
        return RedirectToAction("Reservations");
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult ReturnBook(BookActionModel bookAction)
    {
        using var context = _context;
        var book = context.books!.First(b => b.bookId == bookAction.book);
        book.Return();
        context.SaveChanges();
        TempData["message"] = "Success! Book " + book.title + " returned";
        return RedirectToAction("Borrowings");
    }
}