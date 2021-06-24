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
    public class ExtraFieldRepository : BaseRepository, IExtraFieldRepository
    {
        private readonly ILogger<ExtraFieldRepository> _logger;

        public ExtraFieldRepository(AppDbContext context, ILogger<ExtraFieldRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<ExtraFieldDefinition> AddDefinition(ExtraFieldDefinition definition)
        {
            var result = await _context.ExtraFieldDefinition.AddAsync(definition);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public Task<List<ExtraFieldDefinition>> FindAllDefinitions()
        {
            return _context.ExtraFieldDefinition
                .Include("ExtraFieldDefinitionGudelIdState.GudelIdState")
                .ToListAsync();
        }

        public Task<ExtraFieldDefinition> FindDefinitionByKey(string key)
        {
            return _context.ExtraFieldDefinition
                .Include("ExtraFieldDefinitionGudelIdState.GudelIdState")
                .Where(x => x.Key == key)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public Task<List<ExtraFieldDefinition>> FindDefinitionsByStateId(int stateId)
        {
            return _context.ExtraFieldDefinition
                .Include("ExtraFieldDefinitionGudelIdState.GudelIdState")
                .Where(x => x.ExtraFieldDefinitionGudelIdState.Any(y => y.GudelIdStateId == stateId)).ToListAsync();
        }

        public async Task<int> RemoveDefinition(string key)
        {
            _context.ExtraFieldDefinition.Remove(_context.ExtraFieldDefinition.First(x => x.Key == key));
            return await _context.SaveChangesAsync();
        }

        public async Task<ExtraFieldDefinition> UpdateDefinition(ExtraFieldDefinition definition)
        {
            _context.ExtraFieldDefinitionGudelIdState.RemoveRange(_context.ExtraFieldDefinitionGudelIdState.Where(_ => _.ExtraFieldDefinitionId == definition.Id).ToList());
            _context.ExtraFieldDefinitionGudelIdState.AddRange(definition.ExtraFieldDefinitionGudelIdState);
            var result = _context.ExtraFieldDefinition.Update(definition);
            await _context.SaveChangesAsync();
            return result.Entity;
        }


        public Task<ExtraField> FindExtraField(string gudelId, int extraFieldDefId)
        {
            return _context.ExtraField.Where(x => x.GudelId == gudelId && x.ExtraFieldDefinitionId == extraFieldDefId).FirstOrDefaultAsync();
        }
        public async Task<ExtraField> CreateExtraField(ExtraField extrafield)
        {
            var result = _context.ExtraField.Add(extrafield);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<ExtraField> UpdateExtraField(ExtraField extrafield)
        {
            var result = _context.ExtraField.Update(extrafield);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

    }
}
