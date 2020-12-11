using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repo;
using Services;
using Services.Abstract;
using Services.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCars.Helper
{
    public class EmailEndLotSending
    {
        private IServiceProvider _serviceProvider;
        private UserManager<User> userManager;
        private ICarLotsRepository repository;
        private IEmail email;
        

        public EmailEndLotSending(IServiceProvider serviceProvider, UserManager<User> _userManager, ICarLotsRepository _repository, IEmail _email)
        {
            _serviceProvider = serviceProvider;
            userManager = _userManager;
            repository = _repository;
            email = _email;
        }

        public async Task CheckLot()
        {
            var lots = repository.NotCheckedLots();
            foreach(var lot in lots)
            {
                if(lot.Ending < DateTime.UtcNow)
                {
                   if(lot.WinnerName != null)
                    {
                        var winner = await userManager.FindByNameAsync(lot.WinnerName);
                        var message = $"{winner.UserName}, вы успешно приобрели лот: {lot.Name}";

                        await email.SendEmailAsync(winner.Email, "AuctionCars", message);

                        var messageToOwner = $"{lot.User.UserName}, ваш лот {lot.Name} был приобретён пользователем {winner.UserName}";

                        await email.SendEmailAsync(lot.User.Email, "AuctionCars", messageToOwner);

                    }
                   else
                    {
                        var user = await userManager.FindByNameAsync(lot.User.UserName);
                        var message = $"{user.UserName}, никто не купил ваш лот: {lot.Name}";

                        await email.SendEmailAsync(user.Email, "AuctionCars", message);
                    }

                    lot.Ended = true;
                    repository.UpdateLot(lot);

                }
            }

        }
    }
}
