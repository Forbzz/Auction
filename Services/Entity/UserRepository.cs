using Data;
using Microsoft.EntityFrameworkCore;
using Repo;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Services.Entity
{
   public class UserRepository : IUserRepository
    {
        private ApplicationContext db;
        
        public UserRepository(ApplicationContext _context)
        {
            db = _context;
        }

        public IEnumerable<User> GetAll()
        {
            return db.Users.ToList();
        }

        public User GetUserDb(string id)
        {
            return db.Users.FirstOrDefault(x => x.Id == id);
        }

        public User GetUserInfo(string id)
        {
            return db.Users.Include(u => u.CarLots).ThenInclude(c => c.Car).Include(u => u.Bets).ThenInclude(u => u.CarLot).ThenInclude(u => u.Bets).ThenInclude(b => b.User).FirstOrDefault(u => u.Id == id);
        }

        void IUserRepository.AddUser(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }

        void IUserRepository.DeleteUser(User user)
        {
            db.Users.Remove(user);
            db.SaveChanges();
        }
    }
}
