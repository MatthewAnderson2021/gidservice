using System.Collections.Generic;

namespace GudelIdService.Domain.Models
{
    public class ExtraField
    {
        public int ExtraFieldDefinitionId { get; set; }

        public ExtraFieldDefinition ExtraFieldDefinition { get; set; }

        public Dictionary<string, string> Value { get; set; }

        public string GudelId { get; set; }

        public GudelId Gudel { get; set; }
    }

    public class ExtraFieldDefinition
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public Dictionary<string, string> Name { get; set; }
        public bool IsRequired { get; set; }
        public string Type { get; set; }
        public Dictionary<string, string> Description { get; set; }
        public virtual IEnumerable<ExtraFieldDefinitionGudelIdState> ExtraFieldDefinitionGudelIdState { get; set; }
    }

    public class ExtraFieldDefinitionGudelIdState
    {
        public int ExtraFieldDefinitionId { get; set; }
        public int GudelIdStateId { get; set; }
        public virtual ExtraFieldDefinition ExtraFieldDefinition { get; set; }
        public virtual GudelIdState GudelIdState { get; set; }
    }

    public class FieldTypes
    {
        public readonly string String = "string";
        public readonly string Number = "number";
        public readonly string Boolean = "boolean";
        public readonly string Datetime = "datetime";
    }
}
