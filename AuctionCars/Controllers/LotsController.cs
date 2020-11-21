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
using Newtonsoft.Json;

namespace AuctionCars.Controllers
{
    public class LotsController : Controller
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
        public static bool actual { get; set; }
        const int pageSize = 4;

        public LotsController(ApplicationContext context, CarData carData, ILogger<LotsController> _logger, IHubContext<UpdateHub> _updateHub, UserManager<User> userManager,ICarRepository carRep, ICommentsRepository comm, IBetPerository rep, IWebHostEnvironment appEnviroment, ICarLotsRepository c, ILikesRepository l)
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
        public ActionResult Actual(int? id)
        {
            actual = true;
            int page = id ?? 0;
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_LotsListPart", GetActualItemsPage(page));
            }
            return View("List", GetActualItemsPage(page));
        }


        [HttpGet]
        public ActionResult Ended(int? id)
        {
            actual = false;
            int page = id ?? 0;
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_LotsListPart", GetEndedItemsPage(page));
            }
            return View("List", GetEndedItemsPage(page));
        }



       private IEnumerable<CarLot> GetActualItemsPage(int page = 0)
        {
            int itemsToSkip = page * pageSize;
            return carLotsRepository.ActualLotsPage(itemsToSkip, pageSize);
        }




        private IEnumerable<CarLot> GetEndedItemsPage(int page = 0)
        {
            int itemsToSkip = page * pageSize;

            return carLotsRepository.EndedLotsPage(itemsToSkip, pageSize);
        }


        [HttpGet]
        public ActionResult Load(int? id)
        {
            if (actual == true)
            {
                return Actual(id);
            }
            else
            {
                return Ended(id);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["Message"] = "Создать лот";

            return View();
        }

        [HttpGet]
        [Route("Lot/{id:int}")]
        public IActionResult Detail(int? id)
        {
            if (id != null)
            {
               
                CarLot lot = carLotsRepository.GetDetailLot(id);
                if (lot != null)
                {
                    LotDetailtViewModel lot2 = new LotDetailtViewModel
                    {
                        Lot = lot,
                        BetPrice = lot.Price + 50,
                        BetId = (int)id
                    };
                    return View(lot2);
                }
                logger.LogError("Doesn't exist lot. Controller:Lots. Action:Details");
            }
            logger.LogError("Doesn't exist id. Controller:Lots. Action:Details");
            return RedirectPermanent("~/Error/Index?statusCode=404");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLotViewModel model)
        {
            if (ModelState.IsValid)
            {
                Car car = new Car
                {
                    Name = model.Name,
                    Desc = model.Desc,
                    Year = model.Year,
                    Mileage = model.Milleage,
                    Transmission = (ushort)CarData.transmission.IndexOf(model.Transmission),
                    Fuel = (ushort)CarData.fuel.IndexOf(model.Fuel),
                    Body = (ushort)CarData.body.IndexOf(model.Body),
                    Drive = (ushort)CarData.drive.IndexOf(model.Drive),
                    EngineVolume = double.Parse(model.EngineVolume),
                    Image = null
                };

                CarLot carLot = new CarLot
                {
                    Name = model.Name,
                    StartPrice = model.Price,
                    Price = model.Price,
                    Exposing = DateTime.Now,
                    Ending = DateTime.Now.AddDays(model.Duration),
                    User = await _userManager.GetUserAsync(HttpContext.User),
                    Car = car

                };

                carRepository.AddCar(car);
                
                string src = "/images/" + car.Id + ".jpg";

                using (var fileStream = new FileStream(_appEnviroment.WebRootPath + src, FileMode.Create))
                {
                    await model.Image.CopyToAsync(fileStream);
                }

                car.Image = src;

                carRepository.Update(car);
                carLotsRepository.AddLotDB(carLot);
                


                return RedirectToAction("Actual");
            }
            return View();
        }

        public async Task<IActionResult> Bet(LotDetailtViewModel model)
        {
            CarLot lot = carLotsRepository.GetDetailLot(model.BetId);
            User currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (lot.Price > model.BetPrice || lot.StartPrice > model.BetPrice)
            {
                ModelState.AddModelError(nameof(LotDetailtViewModel.BetPrice), "Ставка должна быть выше текущей");
                return RedirectToAction("Detail", new { id = lot.Id });
            }

            if (ModelState.IsValid)
            {
                User user = await _userManager.GetUserAsync(HttpContext.User);

                Bet bet = new Bet
                {
                    User = user,
                    NewPrice = model.BetPrice,
                    CarLot = lot,
                    Time = DateTime.Now
                };
                if (lot.Bets.Count > 1)
                    currentUser = lot.Bets.Last().User;
                betRep.AddBet(bet);
                lot.Price = model.BetPrice;
                carLotsRepository.UpdateLot(lot);
                

                await updateHub.Clients.AllExcept(user.Id).SendAsync("UpdateTable", lot.Id, bet.User.Id, bet.User.UserName, bet.NewPrice,
                    Convert.ToInt64(bet.Time.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds));
                if(lot.Bets.Count > 1 && currentUser.Email != user.Email)
                {

                    Email email = new Email();
                    await email.SendEmailAsync(currentUser.Email, "CarLots",
                        $"Ваша ставка на товар {lot.Name} была перебита ставкой в {lot.Price} пользователем {user.UserName}");
                }


                return RedirectToAction("Detail", new { id = lot.Id });
            }

           
            
            return RedirectToAction("Detail", new { id = lot.Id });
        }


        public IActionResult Delete(int id)
        {

            var lot = carLotsRepository.GetLotDB(id);
            if (lot == null)
            {
                return NotFound();
            }

            return View(lot);
        }





        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if(id !=null)
            {
                CarLot lot = carLotsRepository.GetLotDB(id);
                if(lot != null)
                {
                    string src = "wwwroot/images" + id + ".jpg";
                    System.IO.File.Delete(src);
                    betRep.DeleteBets(id);
                    carLotsRepository.DeleteLot(lot);

                    return RedirectToAction("Actual");
                }
            }
            logger.LogError("Doesn't exist id. Controller:Lots. Action:Delete");
            return RedirectPermanent("~/Error/Index?statusCode=404");
        }


           
        [Route("Lot/{pbId?}/{comment}")]
        public async Task<IActionResult> CreateComment(int pbId, string comment)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            //comment.User = user;
            Comments comm = new Comments
            {
                CarLotId = pbId,
                User = user,
                Content = comment,
                CarLot = carLotsRepository.GetLotDB(pbId)
            };
            
            if (ModelState.IsValid)
            {
                commRep.AddComm(comm);
            }
            return RedirectToAction("CommentsList", new { id = pbId });
            
        }


        [HttpPost]
        [Route("Lot/Comment/AddLike/{id?}")]
        public async Task<IActionResult> AddLike(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var comm = await db.Comments.FirstOrDefaultAsync(c => c.Id == id);
            Likes like = new Likes
            {
                CommentsId = comm.Id,
                User = user,
                Comments = comm
            };
         
            var find = likesRepository.FindLike(user, comm);
            if(find == null)
            {

                likesRepository.AddLike(like, comm);

            }
            else
            {
                likesRepository.RemoveLike(find,comm);

            }
          
            return RedirectToAction("CommentsList", new { id = comm.CarLotId });
        }

        [HttpPost]  
        [ValidateAntiForgeryToken]
        [Route("Lot/Comment/Delete/{id?}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var comm = await db.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comm == null)
            {
                logger.LogError("Doesn't exist comm. Controller:Lots. Action:DeleteComment");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var lotId = comm.CarLotId;
            if (comm.User != user && !roles.Any(r => r == "admin" || r == "moderator"))
            {
                logger.LogError("You haven't enough authority. Controller:Lots. Action:DeleteComment");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            commRep.RemoveComm(comm);
            await db.SaveChangesAsync();
            return RedirectToAction("CommentsList", new { id = lotId });
        }

        

        public PartialViewResult CommentsList(int id)
        {
            CarLot lot = carLotsRepository.GetDetailLot(id);
            return PartialView(lot.Comments);
        }
    }
}
