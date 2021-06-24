using GudelIdService.Domain.Dto;
using GudelIdService.Domain.Models;
using GudelIdService.Implementation.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GudelIdService.Domain.Services
{
    public interface IGudelIdService
    {
        Task<List<GudelId>> GenerateGudelIds(int amount, string userId);
        Task<GudelId> GenerateGudelId(string userId);
        Task<GudelIdData> Find(string id, string language);
        Task<List<GudelIdData>> FindAll(Expression<Func<GudelId, bool>> query, int pageSize = 1000, int page = 0, string language = ConfigService.LANG_DEFAULT);
        Task<int> FindAllCount(Expression<Func<GudelId, bool>> query);
        Task<GudelId> ReserveGudelId(string id, string userId);
        Task<List<GudelIdData>> CreateGudelIds(GudelIdRequest request, string userId, string language = ConfigService.LANG_DEFAULT);
        Task<int> GetCountForPool(int poolId);
        Task<GudelId> UpdatePoolId(string id, int? poolId);
        Task<GudelId> CreateGudelId(string gudelId, int? poolId, int? typeId, string userId);
        Task<GudelId> ChangeState(GudelIdData olGudelId, int stateId, string userId);
    }
}
