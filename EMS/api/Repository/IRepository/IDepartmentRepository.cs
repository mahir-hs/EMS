using api.Models;

namespace api.Repository.IRepository
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<ApiResponse> UpdateAsync(int id, Department entity);
    }
}
