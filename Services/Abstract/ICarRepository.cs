using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Abstract
{
    public interface ICarRepository
    {
        public void AddCar(Car car);
        public void DeleteCar(Car car);

        public void Update(Car car);
    }
}
