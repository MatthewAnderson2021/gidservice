using GudelIdService.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GudelIdService.Domain.Repositories
{
    public interface IPoolRepository
    {
        Task<List<Pool>> FindAll();
        Task<Pool> FindById(int id);
        Task<Pool> AddAsync(Pool pool);
        Task<Pool> UpdateAsync(Pool pool);
        Task<bool> Remove(Pool pool);

    }
}