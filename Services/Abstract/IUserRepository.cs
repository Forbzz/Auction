using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Abstract
{
    public interface IUserRepository
    {
        public User GetUserDb(string id);
        public void AddUser(User user);
        public void DeleteUser(User user);
        public User GetUserInfo(string id);

        public IEnumerable<User> GetAll();
    }
}
