using System;
using System.Collections.Generic;

namespace GudelIdService.Domain.Dto
{
    public class PoolData
    {
        public int? Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreationDate { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ExternalId { get; set; }

        public int? Size { get; set; }

        public virtual List<GudelIdData> GudelIds { get; set; }
    }
}
