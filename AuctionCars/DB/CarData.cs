using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace AuctionCars.DB
{
    public class CarData
    {
        public static List<string> fuel;
        public static List<string> transmission;
        public static List<string> body;
        public static List<string> drive;

        public CarData(IStringLocalizer<CarData> localizer)
        {
            fuel = new List<string>() { localizer["petrol"], localizer["diesel"], localizer["electro"] };
            transmission = new List<string>() { localizer["automatic"], localizer["manual"] };
            drive = new List<string>() { localizer["rear"], localizer["frontWheel"], localizer["fourWheel"] };
            body = new List<string>() { localizer["SUV"], localizer["cabriolet"], localizer["coupe"], localizer["limousine"], localizer["liftback"], localizer["minibus"], localizer["minivan"], localizer["pickup"], localizer["roadster"], localizer["sedan"], localizer["stationWagon"], localizer["hatchback"], localizer["other"] };
        }
        /*public static string[] fuel = { "дизель", "электро", "бензин" };
        public static string[] transmission = { "автомат", "механика" };
        public static string[] body = { "внедорожник", "кабриолет", "купе", "лимузин", "лифтбек", "микроавтобус", "минивэн", "пикап", "родстер", "седан", "универсал", "хэтчбэк", "другой" };
        public static string[] drive = { "передний", "задний", "полный" };*/

    }
}
