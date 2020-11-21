using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;
using Microsoft.AspNetCore.Identity;

namespace Repo
{
   public static class SeedData
    {
        public static void Initialize(IServiceProvider service)
        {
            using (var db = new ApplicationContext(
                service.GetRequiredService<
                    DbContextOptions<ApplicationContext>>()))
            {
                if(db.CarLots.Any())
                {
                    
                    return;
                }

                else
                {
                    var users = db.Users.FirstOrDefault(u => u.UserName == "admin");
                    db.Cars.AddRange(
                        new Car
                        {
                            Name = "Jeep Grand Cherokee WK2 Laredo",
                            Desc = "Авто в идеальном состоянии, пробег родной, крашенных элементов нет",
                            Image = "/images/car_1.jpg",
                            Year = 2011,
                            Transmission = 0,
                            EngineVolume = 3.6,
                            Fuel = 0,
                            Body = 0,
                            Drive = 2,
                            Mileage = 69000

                        },

                        new Car
                        {
                            Name = "Skoda Octavia II",
                            Desc = "В отличном техническом состоянии",
                            Image = "/images/car_2.jpg",
                            Year = 2010,
                            Transmission = 1,
                            EngineVolume = 1.4,
                            Fuel = 0,
                            Body = 10,
                            Drive = 1,
                            Mileage = 163000

                        }

                    );

                    db.SaveChanges();
                    DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    
                    db.CarLots.AddRange(
                        new CarLot
                        {
                            Name = "Jeep Grand Cherokee WK2 Laredo",
                            StartPrice = 3500,
                            Price = 3500,
                            Exposing = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                            Ending = new DateTime(DateTime.Now.Year, DateTime.Now.Month, ((DateTime.Now.Day + 1)%31), DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                            User = users,
                            Car = db.Cars.FirstOrDefault(c => c.Id == 1),
                            Applyed = true
                        },

                         new CarLot
                         {
                             Name = "Skoda Octavia II",
                             StartPrice = 3500,
                             Price = 3500,
                             Exposing = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                             Ending = new DateTime(DateTime.Now.Year, DateTime.Now.Month, ((DateTime.Now.Day + 2) % 31), DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                             User = users,
                             Car = db.Cars.FirstOrDefault(c => c.Id == 2),
                             Applyed = true
                         }
                    );
                    db.SaveChanges();
                }
            }
        }
    }
}
