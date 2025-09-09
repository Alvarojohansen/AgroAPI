using Application.Dtos;
using Application.Dtos.User;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }


        // Esto es para JWT (tempralmente permanecera aqui, luego lo cambiare.)
        public UserModel ValidationCredentials(CredentialsRequest credentials)
        {
            User? user = GetUserbyEmail(credentials.Email);
            if (user.Password == credentials.Password)
            {
                return new UserModel()
                {
                    Email = user.Email,
                    Id = user.Id,
                    Name = user.Name,
                    Address = user.Address,
                    Phone = user.Phone,
                    City = user.City,
                    Country = user.Country,
                    Role = user.Role
                };
            }else return null;

        }

         
        public List<User> GetAllUser()
        {
            return _repository.GetAllUser();

        }
        public User GetUserbyEmail(string email)
        {
            return _repository.GetByEmail(email);
        }

        public User Get(int Id) 
        {
            return _repository.Get(Id);
        }

        public int AddUser(UserRequest request)
        {
            var user = new User()
            {
                Name =request.Name,
                Email =request.Email,
                Password =request.Password,
                Address =request.Address,
                City = request.City,
                Country =request.Country,
                Phone =request.Phone,
                Role = UserRole.Client
            };
            return _repository.AddUser(user);
        }
        public bool UpdateRoleUser(int id, UserUpdateRoleRequest request) 
        {
            var userUpdate = _repository.Get(id);
            if (userUpdate != null) 
            {
                userUpdate.Role = request.Role;
                _repository.UpdateUser(userUpdate);
                return true;
            }
            return false;
        }

        public bool UpdateUser(int id, UserUpdateRequest request) 
        {
            var userUpdate = _repository.Get(id);
            if (userUpdate != null)
            {
                userUpdate.Name = request.Name;
                userUpdate.Email = request.Email;
                userUpdate.Password = request.Password;
                userUpdate.Address = request.Address;
                userUpdate.City = request.City;
                userUpdate.Country = request.Country;
                userUpdate.Phone = request.Phone;

                _repository.UpdateUser(userUpdate);
                return true;
            }
            return false;

        }

        public void DeleteUser(int id) 
        {
            var userDelete = _repository.Get(id);

            if (userDelete != null) 
            {
                _repository.DeleteUser(userDelete.Id);
            }
        }
    }
}
