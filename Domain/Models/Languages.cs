namespace GudelIdService.Domain.Models
{
    public enum LanguageKey
    {
        deDE,
        enEn,
    }
    public static class LanguageHelper
    {
        public static string ToKeyString(this LanguageKey key)
        {
            return nameof(key).Insert(2, "-");
        }
    }

}
