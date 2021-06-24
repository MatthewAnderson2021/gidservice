using GudelIdService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GudelIdService.Domain.Services
{
    public interface IActivityService
    {
        Task<int> CreateActivityFromIds(GudelId oldGudelId, GudelId newGudelId, string userName);
        Task<Activity> CreateActivity(Activity activity, GudelId gudelId);
        object MapExtraFieldToView(Activity model, string language);
        Task<List<Activity>> FindAll(Expression<Func<Activity, bool>> query);
    }
}
