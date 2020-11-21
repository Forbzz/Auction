using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCars.Controllers
{
    public class RolesController: Controller
    {
        UserManager<User> _userManager;
        public RolesController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        public async Task<IActionResult> SetModerator(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            await _userManager.AddToRoleAsync(user, "moderator");
            return RedirectToAction("Profile", "Account", new { id });
        }

        public async Task<IActionResult> RemoveModerator(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            await _userManager.RemoveFromRoleAsync(user, "moderator");
            return RedirectToAction("Profile", "Account", new { id });
        }


    }
}
