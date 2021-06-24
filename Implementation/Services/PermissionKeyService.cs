using AutoMapper;
using GudelIdService.Domain.Models;
using GudelIdService.Domain.Repositories;
using GudelIdService.Domain.Services;
using GudelIdService.Implementation.Extensions;
using GudelIdService.Implementation.Persistence.Context;
using PasswordGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GudelIdService.Implementation.Services
{
    public class PermissionKeyService : IPermissionKeyService
    {
        public PermissionKeyService(IPermissionKeyRepository permissionKeyRepository, IGudelIdRepository gudelIdRepository, AppDbContext appDbContext, IMapper mapper)
        {
            _permissionKeyRepository = permissionKeyRepository;
            _gudelIdRepository = gudelIdRepository;
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        private IMapper _mapper { get; set; }
        private AppDbContext _appDbContext { get; set; }
        private IGudelIdRepository _gudelIdRepository { get; set; }
        private IPermissionKeyRepository _permissionKeyRepository { get; }

        /// <summary>
        ///     Generates or updates the permissionkey for the given gudelId for every existing permission key type
        /// </summary>
        /// <param name="gudelId"></param>
        /// <returns></returns>
        public async Task<ICollection<PermissionKey>> CreatePermissionKeys(string gudelId)
        {
            var unhashedKeys = new List<string>();
            var permissionKeys = new List<PermissionKey>();
            var exists = await _gudelIdRepository.Find(x => x.Id == gudelId);
            if (exists is null)
            {
                return null;
            }

            //creating permissionKeys
            byte[] gudelIdByteArray = Encoding.ASCII.GetBytes(gudelId);
            for (int i = 0; i < Enum.GetNames(typeof(PermissionKeyType)).Length; i++)
            {
                var password = GetPassword();

                var permissionKey = await GeneratePermissionKey(password, gudelId, gudelIdByteArray, i);
                permissionKeys.Add(permissionKey);
                unhashedKeys.Add(password);
            }

            //update
            var permissionKeyExists = await _permissionKeyRepository.FindPermissionKeyByGudelIdAsync(gudelId);
            if (permissionKeyExists != null)
            {
                foreach (var permissionKey in permissionKeys)
                {
                    await _permissionKeyRepository.UpdatePermissionKeyAsync(permissionKeyExists, permissionKey);
                }
                await _appDbContext.SaveChangesAsync();

                return await GeneratePermissionKeysWithPlainTextPassword(unhashedKeys, permissionKeys);
            }

            //create
            foreach (var permissionKey in permissionKeys)
            {
                await _permissionKeyRepository.CreatePermissionKeyAsync(permissionKey);
            }

            await _appDbContext.SaveChangesAsync();
            return await GeneratePermissionKeysWithPlainTextPassword(unhashedKeys, permissionKeys);
        }

        private string GetPassword() => new Password().IncludeLowercase()
                             .IncludeNumeric()
                             .IncludeUppercase()
                             .LengthRequired(11).Next();

        /// <summary>
        ///     Creates a permission key
        /// </summary>
        /// <param name="password"> the plain password </param>
        /// <param name="gudelId"> the gudelId where we want to create the permission key for </param>
        /// <param name="salt"> hash salt </param>
        /// <param name="permissionTypeFromEnum"> the permissionkeyType where we create the permissionkey for </param>
        /// <returns></returns>
        public async Task<PermissionKey> GeneratePermissionKey(string password, string gudelId, byte[] salt, int permissionTypeFromEnum)
        {
            var hash = CryptographyServiceExtension.HashPassword(password, salt);
            var permissionKey = new PermissionKey()
            {
                GudelId_Id = gudelId,
                Hint = await GenerateHint(password),
                Key = hash,
                Type = (PermissionKeyType)permissionTypeFromEnum
            };

            return permissionKey;
        }

        /// <summary>
        ///     writes the plain password to the permissionkey object collection
        /// </summary>
        /// <param name="unhashedKeys"> the unhashed keys </param>
        /// <param name="permissionKeys"> the permissionKeys with hashed keys </param>
        /// <returns></returns>
        private Task<ICollection<PermissionKey>> GeneratePermissionKeysWithPlainTextPassword(ICollection<string> unhashedKeys, ICollection<PermissionKey> permissionKeys)
        {
            for (int i = 0; i < permissionKeys.Count; i++)
            {
                permissionKeys.ElementAt(i).Key = unhashedKeys.ElementAt(i);
            }

            return Task.FromResult(permissionKeys);
        }

        /// <summary>
        ///     Generates the hint from the plain password, it takes the first 2 chars and fills the rest up with '*'
        /// </summary>
        /// <param name="password"> plain password </param>
        private Task<string> GenerateHint(string password)
        {
            var length = password.Length;
            var firstTwoCharacters = password.Substring(0, 2);
            var hint = firstTwoCharacters;
            for (int i = 0; i < length; i++)
            {
                hint += "*";
            }

            return Task.FromResult(hint);
        }

        /// <summary>
        ///     Gets the key hints for the given gudelId
        /// </summary>
        public async Task<ICollection<Tuple<PermissionKeyType, string>>> GetKeyHintsForGudelIdAsync(string gudelId)
        {
            var permissionKeyTypeHintTupleCollection = new List<Tuple<PermissionKeyType, string>>();

            for (int i = 0; i < Enum.GetNames(typeof(PermissionKeyType)).Length; i++)
            {
                var hint = await _permissionKeyRepository.GetHintByGudelIdAndTypeAsync(gudelId, (PermissionKeyType)i);
                permissionKeyTypeHintTupleCollection.Add(new Tuple<PermissionKeyType, string>((PermissionKeyType)i, hint));
            }
            
            return permissionKeyTypeHintTupleCollection;
        }

        /// <summary>
        ///     Checks if the given gudelId matches with the given key and returns the type and a boolean which indicates if it is valid
        ///     it will return permissionkeytype default when its not valid the default clr type value is 0
        /// </summary>
        public async Task<Tuple<bool, PermissionKeyType>> CheckIfGivenKeyIsAValidKeyOfTheGudelIDAsync(string gudelId, string key)
        {
            byte[] gudelIdByteArray = Encoding.ASCII.GetBytes(gudelId);

            var hash = CryptographyServiceExtension.HashPassword(key, gudelIdByteArray);

            var result = await _permissionKeyRepository.GetPermissionKeyByGudelIdAndKeyAsync(gudelId, hash);

            var type = result != null ? result.Type : default(PermissionKeyType);
            var isValid = false;

            if (result != default)
            {
                isValid = true;
            }

            return new Tuple<bool, PermissionKeyType>(isValid, type);
        }
    }
}
