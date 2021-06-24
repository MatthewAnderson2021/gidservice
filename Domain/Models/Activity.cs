using System;

namespace GudelIdService.Domain.Models
{
    public class Activity
    {
        public int Id { get; set; }

        public Guid Uid { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public string Key { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public bool IsExtraField { get; set; }

        public string GudelId { get; set; }

        public GudelId Gudel { get; set; }
    }
}