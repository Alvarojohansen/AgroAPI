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
        List<User> GetAllUser();
        int AddUser (User user);
        void UpdateUser(User userUpdate);
        void DeleteUser (int id);
    }
}
