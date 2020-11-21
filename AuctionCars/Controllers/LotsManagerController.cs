using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Data;
using AuctionCars.DB;
using Microsoft.EntityFrameworkCore;
using AuctionCars.Components;
using AuctionCars.ViewModel;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Repo;
using Services.Abstract;
using Microsoft.Extensions.Logging;
using Services;
using Microsoft.AspNetCore.SignalR;
using AuctionCars.Hubs;

namespace AuctionCars.Controllers
{
    public class LotsManagerController : Controller
    {
        private ApplicationContext db;
        private UserManager<User> _userManager;
        private ILogger<LotsController> logger;
        private IWebHostEnvironment _appEnviroment;
        private ICarLotsRepository carLotsRepository;
        private ICarRepository carRepository;
        private ILikesRepository likesRepository;
        private IBetPerository betRep;
        private ICommentsRepository commRep;
        private IHubContext<UpdateHub> updateHub;
        const int pageSize = 4;

        public LotsManagerController(ApplicationContext context, CarData carData, ILogger<LotsController> _logger, IHubContext<UpdateHub> _updateHub, UserManager<User> userManager, ICarRepository carRep, ICommentsRepository comm, IBetPerository rep, IWebHostEnvironment appEnviroment, ICarLotsRepository c, ILikesRepository l)
        {
            db = context;
            _userManager = userManager;
            _appEnviroment = appEnviroment;
            carLotsRepository = c;
            likesRepository = l;
            betRep = rep;
            commRep = comm;
            carRepository = carRep;
            logger = _logger;
            updateHub = _updateHub;
        }


        [HttpGet]
        public ActionResult PreModeration(int? id)
        {

            int page = id ?? 0;
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_LotsListPart", GetItemsPage(page));
            }
            return View("PreModeration", GetItemsPage(page));
        }

        private IEnumerable<CarLot> GetItemsPage(int page = 0)
        {
            int itemsToSkip = page * pageSize;
            return carLotsRepository.PremoderationLots(itemsToSkip, pageSize);
        }

        [HttpGet]
        public ActionResult Load(int? id)
        {
            return PreModeration(id);
        }

        public IActionResult Apply(int id)
        {
            CarLot lot = carLotsRepository.GetLotDB(id);
            lot.Applyed = true;
            db.SaveChanges();
            return RedirectToAction("PreModeration");
        }

        public IActionResult Decline(int id)
        {
            if (id != null)
            {
                CarLot lot = carLotsRepository.GetLotDB(id);
                if (lot != null)
                {
                    string src = "wwwroot/images" + id + ".jpg";
                    System.IO.File.Delete(src);
                    betRep.DeleteBets(id);
                    carLotsRepository.DeleteLot(lot);

                    return RedirectToAction("PreModeration");
                }
            }
            logger.LogError("Doesn't exist id. Controller:LotsManager. Action:Decline");
            return RedirectPermanent("~/Error/Index?statusCode=404");
        }

    }
}
