using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Repo;

using System.Linq;


namespace AuctionCars.Components
{
    public class PreModerationLots : ViewComponent
    {
        private ApplicationContext db;
        public PreModerationLots(ApplicationContext _context)
        {
            db = _context;
        }
        public string Invoke()
        {
            var count = db.CarLots.AsEnumerable().Where(l => l.IsActual() && l.Applyed == false).Count();
            if (count == 0)
                return "";

            return $"{count}";
        }
    }
}
