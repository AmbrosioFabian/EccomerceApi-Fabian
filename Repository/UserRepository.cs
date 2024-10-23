﻿using AplicationLayer;
using Data;
using EnterpriseLayer;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetDNIByUserID(string userId)
        {
            return await _context.Users
                        .Where(u => u.Id == userId)
                        .Select(u => u.People.DNI)
                        .FirstOrDefaultAsync() ?? "";
        }

        public async Task<User> GetUserById(string id)
        {
            var userModel = await _context.Users
                .Include(u => u.People)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (userModel == null)
                throw new Exception("Usuario no encontrado.");

            var people = new People(
                userModel.People.DNI,
                userModel.People.Name,
                userModel.People.LastName ?? "",
                userModel.People.Address ?? ""
            );

            return new User(
                userModel.Id,
                userModel.UserName ?? "",
                userModel.Email ?? "",
                people
            );
        }
    }
}
