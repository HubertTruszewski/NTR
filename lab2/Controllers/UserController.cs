using System.Security.Claims;
using System.Text.Json;
using lab2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lab2.Controllers;

public class UserController : Controller
{
    private static List<User>? ReadFromFile()
    {
        using var r = new StreamReader("users.json");
        var json = r.ReadToEnd();
        return JsonSerializer.Deserialize<List<User>>(json);
    }

    private static void WriteToFile(List<User>? users)
    {
        var options = new JsonSerializerOptions() { WriteIndented = true };
        var json = JsonSerializer.Serialize(users, options);
        using var outputFile = new StreamWriter("users.json");
        outputFile.Write(json);
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginModel loginModel)
    {
        var users = ReadFromFile();
        if (users == null)
        {
            ViewBag.Message = "Server error!";
            return Login();
        }

        var user = users.FirstOrDefault(user => user.user == loginModel.user && user.pwd == loginModel.pwd);
        if (user == null)
        {
            ViewBag.Message = "Incorrect username or password!";
            return Login();
        }

        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, loginModel.user!)
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
        var users = ReadFromFile();
        if (users == null)
        {
            ViewBag.Message = "Server error!";
            return Login();
        }

        var user = users.FirstOrDefault(user => user.user == userObject.user && user.pwd == userObject.pwd);
        if (user != null)
        {
            ViewBag.FailureMessage = "This username is already occupied. Choose another one.";
            return View();
        }

        users.Add(userObject);
        var options = new JsonSerializerOptions { WriteIndented = true };
        var serializedUsers = JsonSerializer.Serialize(users, options);
        using (var outputFile = new StreamWriter("users.json"))
        {
            outputFile.Write(serializedUsers);
        }

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
        var users = ReadFromFile();
        var user = users!.First(u => u.user == username);
        using var inputFile = new StreamReader("books.json");
        var json = inputFile.ReadToEnd();
        var books = JsonSerializer.Deserialize<List<Book>>(json);
        var hasLeasedBooks= books!.Where(b => b.IsLeasedForUser(username)).ToList().Any();
        if (hasLeasedBooks)
        {
            TempData["message"] = "Cannot delete account with borrowed books!";
            return RedirectToAction("MyAccount");
        }
        users!.Remove(user);
        WriteToFile(users);
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