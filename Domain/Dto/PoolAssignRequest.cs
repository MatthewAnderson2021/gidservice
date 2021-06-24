namespace GudelIdService.Domain.Dto
{
    public class PoolAssignRequest
    {
        public string GudelId { get; set; }
        public int TargetPoolId { get; set; }
    }
}