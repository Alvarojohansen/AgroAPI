using Application.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public User Get(string name) 
        {
            return _repository.Get(name);
        }

        public int AddUser(UserDtosRequest request)
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
    }
}
