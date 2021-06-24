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
    public class GudelIdStateRepository : BaseRepository, IGudelIdStateRepository
    {
        private readonly ILogger<GudelIdStateRepository> _logger;

        public GudelIdStateRepository(AppDbContext context, ILogger<GudelIdStateRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<List<GudelIdState>> FindAll()
        {
            var result = await _context.GudelIdState.ToListAsync();
            foreach (var state in result)
            {
                state.ExtraFieldDefinition = await _context.ExtraFieldDefinitionGudelIdState.Include(_ => _.ExtraFieldDefinition).Where(_ => _.GudelIdStateId == state.Id).Select(_ => _.ExtraFieldDefinition).ToListAsync();
            }
            return result;
        }

        public async Task<GudelIdState> FindById(int id)
        {
            
            var result =  await _context.GudelIdState.Where(w => w.Id == id).FirstOrDefaultAsync();
            if(result == null)
            {
                return null;
            }
            result.ExtraFieldDefinition = await _context.ExtraFieldDefinitionGudelIdState.Include(_ => _.ExtraFieldDefinition).Where(_ => _.GudelIdStateId == id).Select(_ => _.ExtraFieldDefinition).ToListAsync();
            return result;
        }
    }
}
