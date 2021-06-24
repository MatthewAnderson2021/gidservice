using GudelIdService.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GudelIdService.Domain.Services
{
    public interface IGudelIdStateService
    {
        Task<List<GudelIdStateData>> FindAll(string language);
        Task<GudelIdStateData> FindById(int id, string language);
    }
}
