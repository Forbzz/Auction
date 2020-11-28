using Data;
using Microsoft.EntityFrameworkCore;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using Repo;

namespace Services.Entity
{
    public class CarLotRepository: ICarLotsRepository
    {
        private ApplicationContext db;
        private IRepository<CarLot> carLotRepository;
        public CarLotRepository(ApplicationContext _context, IRepository<CarLot> _carLotRepository)
        {
            db = _context;
            carLotRepository = _carLotRepository;
        }

        public void AddLotDB(CarLot lot)
        {
            carLotRepository.Insert(lot);

        }

        public IEnumerable<CarLot> AllLots()
        {
            return db.CarLots.ToList();
        }

        public void DeleteLot(CarLot lot)
        {

           carLotRepository.Delete(lot);

        }

        public CarLot GetLotDB(int? id)
        {
            return db.CarLots.Where(x => x.Id == id).FirstOrDefault();   
        }

        public IEnumerable<CarLot> MyLots(User user)
        {
            return db.CarLots.Where(x => x.User.Id == user.Id).ToList();   
        }

        public void UpdateLot(CarLot lot)
        {
            carLotRepository.Update(lot);
        }


        public IEnumerable<CarLot> ActualLot()
        {
            return db.CarLots.Include(l => l.User).Include(l => l.Car).AsEnumerable().Where(l => l.IsActual()).OrderBy(l => (l.Ending.Day - DateTime.Now.Day)).ToList();

        }

        public IEnumerable<CarLot> ActualLotsPage(int itemsToSkip, int pageSize)
        {
            return db.CarLots.Include(l => l.User).Include(l => l.Car).AsEnumerable().Where(l => l.IsActual() && l.Applyed == true).OrderBy(l => l.Ending).Skip(itemsToSkip).Take(pageSize).ToList();
        }

        public IEnumerable<CarLot> EndedLotsPage(int itemsToSkip, int pageSize)
        {
            return db.CarLots.Include(l => l.User).Include(l => l.Car).AsEnumerable().Where(l => !l.IsActual() && l.Applyed == true).OrderBy(l => l.Ending).Skip(itemsToSkip).Take(pageSize).ToList();
        }

        public IEnumerable<CarLot> EndedLots()
        {
            return db.CarLots.Include(l => l.User).Include(l => l.Car).AsEnumerable().Where(l => !l.IsActual()).OrderByDescending(l => l.Ending).ToList();
        }

        public CarLot GetDetailLot(int? id)
        {
            return db.CarLots.Include(l => l.User).Include(l=>l.Comments).ThenInclude(c=>c.Likes).Include(l => l.Car).Include(l => l.Bets).ThenInclude(b => b.User).FirstOrDefault(l => l.Id == id);
        }

        public IEnumerable<CarLot> GetUserLots(User user)
        {
            return db.CarLots.Include(l => l.User).Include(l => l.Comments).ThenInclude(c => c.Likes).Include(l => l.Car).Include(l => l.Bets).ThenInclude(b => b.User).Where(l => l.User == user);
        }

        public IEnumerable<CarLot> PremoderationLots(int itemsToSkip, int pageSize)
        {
            return db.CarLots.Include(l => l.User).Include(l => l.Car).AsEnumerable().Where(l => l.IsActual() && l.Applyed == false).OrderBy(l => l.Ending).Skip(itemsToSkip).Take(pageSize).ToList();
        }
    }
}
