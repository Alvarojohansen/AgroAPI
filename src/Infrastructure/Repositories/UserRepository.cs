using Application.Dtos;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }
        public List<User> GetAllUser()
        {
            return _context.Users.ToList();
        }
        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
        public User? Get(int Id)
        {
            return _context.Users.Find(Id);
        }

        public int AddUser(User user) 
        { 
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.Id;
        }
        
        public bool UpdateUser( User user) 
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        public void DeleteUser(int Id) 
        {
            var user = _context.Users.Find(Id);
            if (user != null)
            {
                _context.Users.Remove(user); 
                _context.SaveChanges();
            }
        }
       
    }
}
