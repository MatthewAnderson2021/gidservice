namespace GudelIdService.Domain.Dto
{
    public class GudelIdBatchResult
    {
        public string RequestId { get; set; }
        public object Result { get; set; }
        public bool? Success { get; set; }
        public string Error { get; set; }
    }
}