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
    public class GudelIdRepository : BaseRepository, IGudelIdRepository
    {
        private readonly ILogger<GudelIdRepository> _logger;

        public GudelIdRepository(AppDbContext context, ILogger<GudelIdRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public Task<List<GudelId>> FindAll(int pageSize, int page, Expression<Func<GudelId, bool>> query)
        {
            return _context.GudelId.Where(query).Skip(page * pageSize).Take(pageSize).ToListAsync();
        }

        public Task<int> FindAllCount(Expression<Func<GudelId, bool>> query)
        {
            return _context.GudelId.CountAsync(query);
        }

        public Task<int> Count(Expression<Func<GudelId, bool>> query)
        {
            return _context.GudelId.CountAsync(query);
        }

        public Task<GudelId> Find(Expression<Func<GudelId, bool>> query)
        {
            return _context.GudelId.Include(_ => _.Activities).Include(_ => _.ExtraFields).ThenInclude(_ => _.ExtraFieldDefinition).FirstOrDefaultAsync(query);
        }

        public Task<int> Add(GudelId item)
        {
            _context.GudelId.AddAsync(item);
            return _context.SaveChangesAsync();
        }

        public async Task<GudelId> Update(GudelId item)
        {
            _context.GudelId.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}
