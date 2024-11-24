﻿using api.Models;

namespace api.Repository.IRepository
{
    public interface IDesignationRepository : IRepository<Designation>
    {
        Task<Designation?> UpdateAsync(int id, Designation entity);
    }
}
