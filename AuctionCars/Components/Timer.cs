using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCars.Components
{
    public class Timer: ViewComponent
    {
        public string Invoke(CarLot lot, IViewLocalizer localizer)
        {
            TimeSpan left = lot.Ending - DateTime.Now;
            return $"{left.Days} {localizer["D"].Value}. : {left.Hours} {localizer["H"].Value}. : {left.Minutes} {localizer["M"].Value}.";
        }
    }
}
