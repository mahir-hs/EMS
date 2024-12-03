using api.Models;

namespace api.Repository.IRepository
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<Response<Department?>> UpdateAsync(int id, Department entity);
    }
}
