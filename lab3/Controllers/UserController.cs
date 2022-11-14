using System.Security.Claims;
using lab3.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab3.Controllers;

public class UserController : Controller
{

    private readonly LibraryDbContext _context;

    public UserController(LibraryDbContext context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginModel loginModel)
    {
        using var context = _context;
        var correctLogin = context.users!.Any(u => u.username == loginModel.username && u.pwd == loginModel.pwd);
        if (!correctLogin)
        {
            ViewBag.Message = "Incorrect username or password!";
            return Login();
        }

        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, loginModel.username!)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return LocalRedirect(loginModel.returnUrl ?? "/");
        }
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User userObject)
    {
        using var context = _context;
        var usernameOccupied = context.users!.Any(u => u.username == userObject.username);

        if (usernameOccupied)
        {
            ViewBag.FailureMessage = "This username is already occupied. Choose another one.";
            return View();
        }

        context.users!.Add(userObject);
        context.SaveChanges();

        ViewBag.SuccessMessage = "Success! You can go to login page";
        return View();
    }

    [Authorize]
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return LocalRedirect("/");
    }

    [Authorize]
    [HttpPost]
    public IActionResult DeleteAccount(string username)
    {
        using var context = _context;
        var user = context.users!.First(u => u.username == username);
        var books = context.books!.Include("user").ToList();
        var hasLeasedBooks = books.Where(b => b.IsLeasedForUser(username)).ToList().Any();
        if (hasLeasedBooks)
        {
            TempData["message"] = "Cannot delete account with borrowed books!";
            return RedirectToAction("MyAccount");
        }

        var reservedBooksForUser = books.Where(b => b.IsReservedForUser(username)).ToList();
        foreach (var book in reservedBooksForUser)
        {
            book.CancelReservation();
        }
        context.users!.Remove(user);
        context.SaveChanges();
        TempData["message"] = "The account has been deleted";
        return RedirectToAction("Logout");
    }

    [Authorize]
    public IActionResult MyAccount()
    {
        ViewBag.Message = TempData["message"]!;
        return View();
    }
}