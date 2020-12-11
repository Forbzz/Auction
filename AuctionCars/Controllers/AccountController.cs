using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuctionCars.ViewModel;
using Data;
using Microsoft.AspNetCore.Identity;
using AuctionCars.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Repo;
using Services.Abstract;
using Services;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace AuctionCars.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private ILogger<AccountController> logger;
        private IUserRepository userRep;
        private ICarLotsRepository carRep;
        private IBetPerository betRep;
        private IEmail Email;
        private ApplicationContext db;
        public AccountController(UserManager<User> userManager,ApplicationContext _context, IEmail _Email ,IBetPerository betRerository ,SignInManager<User> signInManager,ICarLotsRepository carRepository ,ILogger<AccountController> _logger, ApplicationContext context, IUserRepository repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            logger = _logger;
            userRep = repository;
            carRep = carRepository;
            db = _context;
            betRep = betRerository;
            Email = _Email;
        }

        [AllowAnonymous]
        [HttpGet] 
        public async Task<IActionResult> LoginAsync(string returnUrl)
         {

            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExtrernalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);

         }

       

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
                                new { ReturnUrl = returnUrl });
            var properties = _signInManager
                .ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var userName = model.Email;
                var user = await _userManager.FindByEmailAsync(userName);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Такого пользователя не существует");
                    return View(model);
                }
                else
                {
                    userName = user.UserName;
                }
                if(model.Remember)
                {
                    var result = await _signInManager.PasswordSignInAsync(userName, model.Password, isPersistent: true, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Actual", "Lots");
                    }
                }
                else
                {
                    var result = await _signInManager.PasswordSignInAsync(userName, model.Password, isPersistent: false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Actual", "Lots");
                    }
                }
                
                
            }
            else
            {
                ModelState.AddModelError("", "Неверный пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Name,
                    Registration = DateTime.Now.ToUniversalTime(),
                 
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "guest");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

                    var url = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    await Email.SendEmailAsync(model.Email, "Confirm your account",
                        $"Подтвердите аккаунт, перейдя по ссылке: <a href='{url}'>link</a>");
                   
                    return RedirectToAction("Message"); 
                }

                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
            }
            return View(model);
        }

        public IActionResult Message()
        {
            return View();
        }


        [AllowAnonymous]
        public async Task<IActionResult>
        ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExtrernalLogins =
                        (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState
                    .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }

            
            // Получаем информации о пользователе зашедшего через внешнего провайдера
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState
                    .AddModelError(string.Empty, "Error loading external login information.");

                return View("Login", loginViewModel);
            }

            
            //Если пользователь уже зарегистрирован в приложении, то просто заходим
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            
            else
            {
                
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        string userName = info.Principal.FindFirstValue(ClaimTypes.Email);
                        string result = userName.Substring(0, userName.LastIndexOf('@'));
                        user = new User
                        {
                            UserName = result,
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Registration = DateTime.UtcNow,
                            EmailConfirmed = true
                        };

                        await _userManager.CreateAsync(user, "temporary");
                        await _signInManager.PasswordSignInAsync(user, "temporary", isPersistent: false, lockoutOnFailure: false);

                    }
                    var res = await _signInManager.PasswordSignInAsync(user, "temporary", isPersistent: false, lockoutOnFailure: false);
                    if (res.Succeeded)
                    {
                        return RedirectToAction("CreatePassword");
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, true);
                        return RedirectToAction("Actual", "Lots");
                    }
                    
                }

                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                

                return View("Error");
            }
        }



        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
         
            User user = userRep.GetUserInfo(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            User currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ProfileViewModel obj = new ProfileViewModel
            {
                user = user,
                isMe = (currentUser != null && currentUser.Id == id) ? true : false
            };
            return View(obj);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    IdentityResult result =
                    await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
             
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Profile");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if(userId == null || code == null)
            {
                logger.LogError("Wrong userdId or code. Controller:Account. Action:ConfirmEmail");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                logger.LogError("Doesn't exist user. Controller:Account. Action:ConfirmEmail");
                return RedirectPermanent("~/Error/Index?statusCode=404");

            }
            await _userManager.RemoveFromRoleAsync(user, "guest");
            await _userManager.AddToRoleAsync(user, "user");
            await _signInManager.SignInAsync(user, false);

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("Actual", "Lots");
            else
                return RedirectPermanent("~/Error/Index?statusCode=404");
        }

        public async Task<IActionResult> DeleteUser(string id)
        {

            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var lots = carRep.GetUserLots(user);
                List<Bet> userBets = db.Bets.Include(b => b.CarLot).Include(b => b.CarLot.Bets).Where(b => b.User.Id == id || b.CarLot.User.Id == id).ToList();
                
                db.Bets.RemoveRange(db.Bets.Where(b => b.User.Id == id || b.CarLot.User.Id == id));
                db.SaveChanges();    
                foreach (Bet bet in userBets)
                {
                    CarLot stavkiOnCarLotUsera = bet.CarLot;

                        if (stavkiOnCarLotUsera.Bets.Count != 0)
                            bet.CarLot.Price = stavkiOnCarLotUsera.Bets.Last().NewPrice;
                        else
                            bet.CarLot.Price = bet.CarLot.StartPrice;

                }
                

                foreach (CarLot lot in lots)
                {

                    string path = "wwwroot/images/" + lot.Id + ".jpg";
                    System.IO.File.Delete(path);
                }
                db.CarLots.RemoveRange(db.CarLots.Where(l => l.User.Id == id));
                db.Users.Remove(user);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Users");
        }

        [HttpGet]
        public IActionResult CreatePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePassword(CreatePasswordViewModel model)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            IdentityResult result =
                    await _userManager.ChangePasswordAsync(user, "temporary", model.NewPassword);

            if (result.Succeeded)
            {
                return RedirectToAction("Actual", "Lots");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

    }


}
