using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
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
        public User? Get(string name)
        {
            return _context.Users.FirstOrDefault(u => u.Name == name);
        }

        public int AddUser(User user) 
        { 
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.Id;
        }
       
    }
}
