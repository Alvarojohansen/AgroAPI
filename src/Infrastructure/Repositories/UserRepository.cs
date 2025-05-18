using Application.Dtos;
using Domain.Entities;
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
        public User GetByEmail(string Email)
        {
            return _context.Users.Find(Email);
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
        public void UpdateUser( User user) 
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
            return;
        }

        public void DeleteUser(int Id) 
        {
            var user = _context.Users.Find(Id); // Buscar el usuario por su ID
            if (user != null)
            {
                _context.Users.Remove(user);    // Eliminar el objeto, no el ID
                _context.SaveChanges();         // Guardar los cambios
            }
        }
       
    }
}
