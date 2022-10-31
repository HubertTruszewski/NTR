using System.Text.Json;
using lab1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lab1.Controllers;

public class BookController : Controller
{
    private static List<Book>? ReadFromFile()
    {
        using var inputFile = new StreamReader("books.json");
        var json = inputFile.ReadToEnd();
        return JsonSerializer.Deserialize<List<Book>>(json);
    }

    private static void WriteToFile(List<Book>? books)
    {
        var options = new JsonSerializerOptions() { WriteIndented = true };
        var json = JsonSerializer.Serialize(books, options);
        using var outputFile = new StreamWriter("books.json");
        outputFile.Write(json);
    }
    
    [Authorize]
    public IActionResult List()
    {
        var books = ReadFromFile();
        ViewBag.Message = TempData["message"]!;
        return View(books);
    }

    [HttpPost]
    public IActionResult List(string phrase)
    {
        var books = ReadFromFile();
        if (string.IsNullOrEmpty(phrase))
        {
            return View(books);
        }
        books = books!.Where(b => b.title!.ToLower().Contains(phrase.ToLower())).ToList();
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
        var reservedBooks = ReadFromFile()!.Where(b => b.IsReserved()).ToList();
        ViewBag.Message = TempData["message"]!;
        return View(reservedBooks);
    }
    
    [Authorize]
    public IActionResult Borrowings()
    {
        var borrowedBooks = ReadFromFile()!.Where(b => b.IsLeased()).ToList();
        ViewBag.Message = TempData["message"]!;
        return View(borrowedBooks);
    }

    [Authorize]
    [HttpPost]
    public IActionResult ReserveBook(BookActionModel bookAction)
    {
        var books = ReadFromFile();
        var book = books!.First(b => b.id == bookAction.book);
        book.reserved = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        book.user = bookAction.user;
        WriteToFile(books);
        TempData["message"] = "Success! You reserved a book: " + book.title;
        return RedirectToAction("List");
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult CancelReservation(BookActionModel bookAction)
    {
        var books = ReadFromFile();
        var book = books!.First(b => b.id == bookAction.book);
        book.reserved = "";
        book.user = "";
        WriteToFile(books);
        TempData["message"] = "You canceled reservation for book: " + book.title;
        return RedirectToAction("Reservations");
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult BorrowBook(BookActionModel bookAction)
    {
        var books = ReadFromFile();
        var book = books!.First(b => b.id == bookAction.book);
        book.leased = DateTime.Now.AddDays(14).ToString("yyyy-MM-dd");
        book.reserved = "";
        WriteToFile(books);
        TempData["message"] = "Success! Book " + book.title + " borrowed";
        return RedirectToAction("Reservations");
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult ReturnBook(BookActionModel bookAction)
    {
        var books = ReadFromFile();
        var book = books!.First(b => b.id == bookAction.book);
        book.leased = "";
        book.user = "";
        WriteToFile(books);
        TempData["message"] = "Success! Book " + book.title + " returned";
        return RedirectToAction("Borrowings");
    }
}