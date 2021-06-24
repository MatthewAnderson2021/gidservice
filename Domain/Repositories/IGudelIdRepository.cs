using GudelIdService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GudelIdService.Domain.Repositories
{
    public interface IGudelIdRepository
    {
        Task<int> Add(GudelId item);
        Task<GudelId> Update(GudelId item);
        Task<int> Count(Expression<Func<GudelId, bool>> query);
        Task<GudelId> Find(Expression<Func<GudelId, bool>> query);
        Task<List<GudelId>> FindAll(int pageSize, int page, Expression<Func<GudelId, bool>> query);
        Task<int> FindAllCount(Expression<Func<GudelId, bool>> query);
    }
}
