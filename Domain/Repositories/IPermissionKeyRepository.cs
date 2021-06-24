using GudelIdService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GudelIdService.Domain.Repositories
{
    public interface IPermissionKeyRepository
    {
        Task<PermissionKey> CreatePermissionKeyAsync(PermissionKey permissionKey);
        Task<PermissionKey> FindPermissionKeyByGudelIdAsync(string gudelId);
        Task<PermissionKey> UpdatePermissionKeyAsync(PermissionKey entity, PermissionKey valueContainer);
        Task<string> GetHintByGudelIdAndTypeAsync(string gudelId, PermissionKeyType permissionKeyType);
        Task<PermissionKey> GetPermissionKeyByGudelIdAndKeyAsync(string gudelId, string hashedKey);
    }
}
