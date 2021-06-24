using GudelIdService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GudelIdService.Domain.Repositories
{
    public interface IActivityRepository
    {
        Task<Activity> AddAsync(Activity activity);

        Task<List<Activity>> FindAll(Expression<Func<Activity, bool>> query);
    }
}