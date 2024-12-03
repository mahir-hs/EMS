using api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services.IServices
{
    public interface IService<T> where T:class
    {
        Task<Response<IEnumerable<T?>>> GetAllAsync();
        Task<Response<T?>> GetByIdAsync(int id);
        Task<Response<T>> DeleteAsync(int id);
    }
}




