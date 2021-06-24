using System;
using System.Collections.Generic;

namespace GudelIdService.Domain.Models
{
    public class GudelId
    {
        public string Id { get; set; }

        public int StateId { get; set; }

        public GudelIdState State { get; set; }

        public int TypeId { get; set; }

        public GudelIdType Type { get; set; }

        public int? PoolId { get; set; }

        public Pool Pool { get; set; }

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

        public Dictionary<string, string> Description { get; set; }

        public Dictionary<string, string> Name { get; set; }

        public ICollection<Activity> Activities { get; set; }

        public ICollection<ExtraField> ExtraFields { get; set; }
        public ICollection<PermissionKey> PermissionKeys { get; set; }

        public void AddActivity(Activity activity)
        {
            //if (!activity.id)
            //{
            // await getConnection()
            //   .createQueryBuilder()
            //   .insert()
            //   .into(Activity)
            //   .values([activity])
            //   .execute();
            //}
            // await getConnection()
            //   .createQueryBuilder()
            //   .relation(GudelId, 'activities')
            //   .of(this)
            //   .add(activity);
        }

    }
}

