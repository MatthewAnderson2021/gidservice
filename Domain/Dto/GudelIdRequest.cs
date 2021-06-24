namespace GudelIdService.Domain.Dto
{
    public class GudelIdRequest
    {
        public int Amount { get; set; }
        public int? poolId { get; set; }
        public int TypeId { get; set; }
    }
}