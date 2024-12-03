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
        Task<Response<IEnumerable<T?>>> GetAllAsync();
        Task<Response<T?>> GetByIdAsync(int id);
        Task<Response<T?>> AddAsync(T entity);
        Task<Response<T?>> DeleteAsync(int id);
    }
}