using GudelIdService.Domain.Dto;
using GudelIdService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GudelIdService.Domain.Repositories
{
    public interface IPoolService
    {
        Task<List<PoolData>> FindAll(string language);
        Task<PoolData> FindById(int id, string language);
        Task<PoolData> AddAsync(PoolData pool, string language);
        Task<PoolData> Update(PoolData pool, string language);
        Task<bool> Remove(int id);

    }
}