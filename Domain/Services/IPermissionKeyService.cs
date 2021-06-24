using GudelIdService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GudelIdService.Domain.Services
{
    public interface IPermissionKeyService
    {
        Task<ICollection<PermissionKey>> CreatePermissionKeys(string gudelId);
        Task<ICollection<Tuple<PermissionKeyType, string>>> GetKeyHintsForGudelIdAsync(string gudelId);
        Task<Tuple<bool, PermissionKeyType>> CheckIfGivenKeyIsAValidKeyOfTheGudelIDAsync(string gudelId, string key);
        Task<PermissionKey> GeneratePermissionKey(string password, string gudelId, byte[] salt, int permissionTypeFromEnum);
    }
}
