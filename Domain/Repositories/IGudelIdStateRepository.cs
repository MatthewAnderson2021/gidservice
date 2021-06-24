using GudelIdService.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GudelIdService.Domain.Repositories
{
    public interface IGudelIdStateRepository
    {
        Task<List<GudelIdState>> FindAll();
        Task<GudelIdState> FindById(int id);
    }
}