using System.Collections.Generic;

namespace GudelIdService.Domain.Dto
{
    public class GudelIdBatchRequests
    {
        public List<string> GudelIds { get; set; }

        public Dictionary<string, string> ExtraFieldData { get; set; }
    }
}
