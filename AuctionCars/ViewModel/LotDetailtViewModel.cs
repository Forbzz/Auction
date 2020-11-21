using Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCars.ViewModel
{
    public class LotDetailtViewModel
    {
        public CarLot Lot { get; set; }
        public int BetId { get; set; }

        [Required(ErrorMessage = "Укажите размер ставки")]
        public uint BetPrice { get; set; }
        public User getWinner()
        {
            return Lot.Bets.Last().User;
        }

    }
}
