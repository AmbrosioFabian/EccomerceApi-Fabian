﻿using Data.Entity;

namespace EccomerceApi.Interfaces
{
    public interface IState
    {
        Task<List<State>> GetAllAsync();
    }
}
