using System.Collections.Generic;

namespace GudelIdService.Domain.Dto
{
    public class GudelIdStateData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<ExtraFieldDefinitionData> ExtraFieldDefinitions { get; set; }

        public List<int> AllowedFollowupStateIds { get; set; }

        public List<GudelIdStateData> AllowedFollowupStates { get; set; }

        public List<int> PossiblePreviousStateIds { get; set; }

        public List<GudelIdStateData> PossiblePreviousStates { get; set; }
    }
}
