using api.Models;

namespace api.Repository.IRepository
{
    public interface IDesignationRepository : IRepository<Designation>
    {
        Task<ApiResponse> UpdateAsync(int id, Designation entity);
    }
}
