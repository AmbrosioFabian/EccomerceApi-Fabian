﻿using EccomerceApi.Model;

namespace EccomerceApi.Interfaces
{
    public interface IUserService
    {
        Task<List<UserModel>> GetAllUsers();

        Task<UserModel> GetUserByEmail(string emailId);

        Task<bool> UpdateUser(string emailId, UserModel user);

        Task<bool> DeleteUserByEmail(string emailId);

        Task<bool> BlockUserAsync(string emailId);
    }
}
