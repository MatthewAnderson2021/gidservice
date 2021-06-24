using GudelIdService.Domain.Models;
using GudelIdService.Domain.Repositories;
using GudelIdService.Implementation.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GudelIdService.Implementation.Persistence.Repository
{
    public class PermissionKeyRepository : BaseRepository, IPermissionKeyRepository
    {

        public PermissionKeyRepository(AppDbContext context) : base(context){}

        public async Task<PermissionKey> CreatePermissionKeyAsync(PermissionKey permissionKey)
        {
            await _context.PermissionKeys.AddAsync(permissionKey);

            return permissionKey;
        }

        public async Task<PermissionKey> FindPermissionKeyByGudelIdAsync(string gudelId)
        {
            return await _context.PermissionKeys.Where(permissionKey => permissionKey.GudelId_Id == gudelId)
                                                .FirstOrDefaultAsync();
        }

        public async Task<string> GetHintByGudelIdAndTypeAsync(string gudelId, PermissionKeyType permissionKeyType)
        {
            return await _context.PermissionKeys.Where(permissionKey => permissionKey.GudelId_Id == gudelId && permissionKey.Type == permissionKeyType)
                                                .Select(permissionKey => permissionKey.Hint)
                                                .FirstOrDefaultAsync();
        }

        public async Task<PermissionKey> GetPermissionKeyByGudelIdAndKeyAsync(string gudelId, string hashedKey)
        {
            return await _context.PermissionKeys.Where(permissionKey => permissionKey.GudelId_Id == gudelId && permissionKey.Key == hashedKey)
                                                .FirstOrDefaultAsync();
        }

        public Task<PermissionKey> UpdatePermissionKeyAsync(PermissionKey entity, PermissionKey valueContainer)
        {
            _context.Entry(entity).State = EntityState.Detached;
            var result = _context.PermissionKeys.Update(valueContainer);

            return Task.FromResult(result.Entity);
        }
    }
}
