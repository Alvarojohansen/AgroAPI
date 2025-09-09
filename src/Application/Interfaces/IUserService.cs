using Application.Dtos.User;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        List<User> GetAllUser();
        User? GetUserbyEmail(string email);
        User? Get(int Id);
        int AddUser(UserRequest body);
        bool UpdateUser(int id, UserUpdateRequest user);
        bool UpdateRoleUser(int id, UserUpdateRoleRequest user);
        void DeleteUser(int id);
        
    }
}
