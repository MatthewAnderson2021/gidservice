using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GudelIdService.Domain.Models
{
    public class PermissionKey
    {
        public string Key { get; set; } // generated & hashed key (hash probably key+gudelid)
        public string Hint { get; set; } // the first two chars of the original key, the rest obfuscated with * -> eg. TYmB7oLxKxI -> TY*********
        public PermissionKeyType Type { get; set; } // PermissionKeyType
        public GudelId GudelId { get; set; }  //navigation property
        public string GudelId_Id { get; set; } //Foreign Key to GudelId.Id

        // ensure that there can only be one key per GudelId+Type
    }

    //generate key for every entry
    public enum PermissionKeyType
    {
        RESELLER,
        CUSTOMER,
    }
}
