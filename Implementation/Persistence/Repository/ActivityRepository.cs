using GudelIdService.Domain.Models;
using GudelIdService.Domain.Repositories;
using GudelIdService.Implementation.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GudelIdService.Implementation.Persistence.Repository
{
    public class ActivityRepository : BaseRepository, IActivityRepository
    {
        private readonly ILogger<ActivityRepository> _logger;

        public ActivityRepository(AppDbContext context, ILogger<ActivityRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<Activity> AddAsync(Activity activity)
        {
            await _context.Activity.AddAsync(activity);

            return activity;
        }

        public async Task<List<Activity>> FindAll(Expression<Func<Activity, bool>> query)
        {
            return await _context.Activity
                .Include(_ => _.Gudel)
                .Where(query).ToListAsync();
        }
    }
}
