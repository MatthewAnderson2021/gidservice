namespace GudelIdService.Domain.Services
{
    public interface IConfigService
    {
        string Get(string key);
        int HttpPort();
        int CreateCronInterval();
        int CreateCronAmount();
        int MaxBodyParseLimit();
    }
}

