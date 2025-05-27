using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {

        User? Get(int Id);
        User? GetByEmail(string Email);
        List<User> GetAllUser();
        int AddUser (User user);
        bool UpdateUser(User userUpdate);
        void DeleteUser (int id);
    }
}
