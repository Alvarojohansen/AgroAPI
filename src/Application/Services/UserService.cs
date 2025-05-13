using Application.Dtos;
using Domain.Entities;
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
    public class UserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        public List<User> GetAllUser()
        {
            return _repository.GetAllUser();

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
                Country =request.Country,
                Phone =request.Phone,
            };
            return _repository.AddUser(user);
        }

        public void UpdateUser(int id, UserUpdateRequest request) 
        {
            var userUpdate = _repository.Get(id);
            if (userUpdate != null)
            {
                userUpdate.Name = request.Name;
                userUpdate.Email = request.Email;
                userUpdate.Password = request.Password;
                userUpdate.Address = request.Address;
                userUpdate.Country = request.Country;
                userUpdate.Phone = request.Phone;

                _repository.UpdateUser(userUpdate);
            }
            
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
