using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCars.Views
{
    public static class DateExtension
    {
        public static string ToClientTime(this DateTime dt, ISession session)
        {
            var timeOffSet = session.GetInt32("timezoneoffset");

            if (timeOffSet != null)
            {
                var offset = int.Parse(timeOffSet.ToString());
                dt = dt.AddMinutes(-1 * offset);
                return dt.ToString();
            }
            return dt.ToLocalTime().ToString();
        }
    }
}
