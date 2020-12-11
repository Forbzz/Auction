using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCars.Helper
{
    public static class DateExtension
    {
        public static string ToClientTime(this DateTime dt, HttpContext context)
        {
            var timeOffSet = context.Request.Cookies["timezoneoffset"];

            if (timeOffSet != null)
            {
                var offset = int.Parse(timeOffSet);
                dt = dt.AddMinutes(-1 * offset);
                return dt.ToString();
            }
            return dt.ToLocalTime().ToString();
        }
    }
}
