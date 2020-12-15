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
        private ILogger<LotsController> logger;
        private ICarLotsRepository carLotsRepository;
        private IBetPerository betRep;


        public LotsManagerController(ApplicationContext context, CarData carData, ILogger<LotsController> _logger, IBetPerository rep,  ICarLotsRepository c)
        {
            db = context;
            carLotsRepository = c;
            betRep = rep;
            logger = _logger;

        }

        [HttpGet]
        public IActionResult PreModeration()
        {
            return View(carLotsRepository.PremoderationLots());
        }

        public IActionResult Apply(int id)
        {
            CarLot lot = carLotsRepository.GetLotDB(id);
            lot.Applyed = true;
            db.SaveChanges();
            return RedirectToAction("PreModeration");
        }

        public IActionResult Decline(int? id)
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
