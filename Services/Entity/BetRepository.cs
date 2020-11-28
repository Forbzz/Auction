using Data;
using Repo;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Entity
{
   public class BetRepository : IBetPerository
    {
        private ApplicationContext db;
        private IRepository<Bet> betRep;

        public BetRepository(ApplicationContext _context, IRepository<Bet> repository)
        {
            db = _context;
            betRep = repository;
        }

        public void DeleteBets(int? id)
        {
            db.Bets.RemoveRange(db.Bets.Where(b => b.CarLot.Id == id));
            db.SaveChanges();
        }

        void IBetPerository.AddBet(Bet bet)
        {
            betRep.Insert(bet);
            db.SaveChanges();
        }


        void IBetPerository.DeleteBet(Bet bet)
        {

            betRep.Delete(bet);
            db.SaveChanges();
        }



    }
}
