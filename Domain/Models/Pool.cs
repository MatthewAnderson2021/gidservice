using System;
using System.Collections.Generic;

namespace GudelIdService.Domain.Models
{
    public class Pool
    {
        public int Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public Dictionary<string, string> Name { get; set; }

        public Dictionary<string, string> Description { get; set; }

        public int Size { get; set; }

        public string ExternalId { get; set; }

        public virtual List<GudelId> GudelIds { get; set; }

    }
}
