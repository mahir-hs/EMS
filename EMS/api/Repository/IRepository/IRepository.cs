using api.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Repository.IRepository
{
    public interface IRepository<T> where T:class
    {
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> AddAsync(T entity);
        Task<ApiResponse> DeleteAsync(int id);
    }
}