using Data;
using Repo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Abstract
{
    public interface  ICarLotsRepository
    {
        public void AddLotDB(CarLot lot);
        public IEnumerable<CarLot> AllLots();
        public IEnumerable<CarLot> MyLots(User user);
        public CarLot GetLotDB(int id);
        public void UpdateLot(CarLot lot);
        public void DeleteLot(CarLot lot);
        public IEnumerable<CarLot> ActualLot();
        public IEnumerable<CarLot> ActualLotsPage(int itemsToSkip, int pageSize);
        public IEnumerable<CarLot> EndedLotsPage(int itemsToSkip, int pageSize);
        public IEnumerable<CarLot> PremoderationLots(int item, int pageSize);

        public IEnumerable<CarLot> GetUserLots(User user);
        public CarLot GetDetailLot(int? id);
        public IEnumerable<CarLot> EndedLots();
    }
}
