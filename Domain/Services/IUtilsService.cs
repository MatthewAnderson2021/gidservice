namespace GudelIdService.Domain.Services
{
    public interface IUtilsService
    {
        string GenerateRandomString(int size, bool lowerCase);
    }
}
