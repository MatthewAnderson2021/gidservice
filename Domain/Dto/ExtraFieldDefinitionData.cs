using System.Collections.Generic;

namespace GudelIdService.Domain.Dto
{
    public class ExtraFieldDefinitionData
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public List<int> State { get; set; }

        public bool IsRequired { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }
}
