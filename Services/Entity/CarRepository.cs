using Data;

using System;
using System.Collections.Generic;
using Repo;
using Services.Abstract;

namespace Services.Entity
{
    public class CarRepository : ICarRepository
    {
        private ApplicationContext db;
        private IRepository<Car> carRep;

        public CarRepository(ApplicationContext _context, IRepository<Car> repository)
        {
            db = _context;
            carRep = repository;
        }

        public void Update(Car car)
        {
            carRep.Update(car);
        }

        void ICarRepository.AddCar(Car car)
        {
            carRep.Insert(car);
            
        }

        void ICarRepository.DeleteCar(Car car)
        {
            carRep.Delete(car);
            
        }
    }
}
