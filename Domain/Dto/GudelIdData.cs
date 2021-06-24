using GudelIdService.Domain.Models;
using System;
using System.Collections.Generic;

namespace GudelIdService.Domain.Dto
{
    public class GudelIdData
    {
        public string Id { get; set; }

        public int StateId { get; set; }

        public int TypeId { get; set; }

        public int? PoolId { get; set; }

        public string PrivateKey { get; set; }

        public string OwnerKey { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? ReservationDate { get; set; }

        public DateTime? ProductionDate { get; set; }

        public DateTime? AssignmentDate { get; set; }

        public DateTime? VoidDate { get; set; }

        public string CreatedBy { get; set; }

        public string ReservedBy { get; set; }

        public string ProducedBy { get; set; }

        public string AssignedBy { get; set; }

        public string VoidedBy { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public List<Activity> Activities { get; set; }

        public List<ExtraField> ExtraFields { get; set; }
    }
}
