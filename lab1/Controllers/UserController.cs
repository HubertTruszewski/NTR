using System.Security.Claims;
using System.Text.Json;
using lab1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lab1.Controllers;

public class UserController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginModel loginModel)
    {
        List<User>? users;
        using (var r = new StreamReader("users.json")) {
            var json = r.ReadToEnd();
            users = JsonSerializer.Deserialize<List<User>>(json);
        }
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
        List<User>? users;
        using (var r = new StreamReader("users.json")) {
            var json = r.ReadToEnd();
            users = JsonSerializer.Deserialize<List<User>>(json);
        }
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
        using (var w = new StreamWriter("users.json"))
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var serializedUsers = JsonSerializer.Serialize(users, options);
            w.Write(serializedUsers);
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
}