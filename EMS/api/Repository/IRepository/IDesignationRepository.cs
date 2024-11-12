using api.Models;

namespace api.Repository.IRepository
{
    public interface IDesignationRepository : IRepository<Designation>
    {
        Task<Designation?> UpdateAsync(Designation entity);
    }
}
