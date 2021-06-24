using GudelIdService.Domain.Models;
using GudelIdService.Domain.Repositories;
using GudelIdService.Implementation.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GudelIdService.Implementation.Persistence.Repository
{
    public class PoolRepository : BaseRepository, IPoolRepository
    {
        private readonly ILogger<PoolRepository> _logger;

        public PoolRepository(AppDbContext context, ILogger<PoolRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<Pool> AddAsync(Pool pool)
        {
            await _context.Pool.AddAsync(pool);
            await _context.SaveChangesAsync();
            return pool;
        }

        public async Task<List<Pool>> FindAll()
        {
            return await _context.Pool.ToListAsync();
        }

        public async Task<Pool> FindById(int id)
        {
            return await _context.Pool.Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> Remove(Pool pool)
        {
            _context.Pool.Remove(pool);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Pool> UpdateAsync(Pool pool)
        {
            var localPool = _context.Pool.Local.FirstOrDefault(_ => _.Id == pool.Id);
            if (localPool != null) _context.Entry(localPool).State = EntityState.Detached;
            var result = _context.Pool.Update(pool);
            await _context.SaveChangesAsync();
            return result.Entity;

        }
    }
}
