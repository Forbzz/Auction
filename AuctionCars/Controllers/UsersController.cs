using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionCars.DB;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repo;
using Services.Abstract;

namespace AuctionCars.Controllers
{
    public class UsersController : Controller
    {

        UserManager<User> _userManager;
        ApplicationContext db;
        IUserRepository rep;

        public UsersController(IUserRepository _rep ,UserManager<User> userManager, ApplicationContext contex)
        {
            _userManager = userManager;
            db = contex;
            rep = _rep;
        }

        [HttpGet]
        
        public async Task<IActionResult> Profile()
        {
            User currentUser = await _userManager.GetUserAsync(HttpContext.User);
            return RedirectToAction("Profile", "Account", new { id = currentUser.Id });
            
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "Все пользователи";
            return View(_userManager.Users.ToList());
        }

       
    }
}
