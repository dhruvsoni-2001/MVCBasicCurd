// Services/AccountNumberGenerator.cs
namespace MVCBasicCurd.Services
{
    public static class AccountNumberGenerator
    {
        // Example: "BANK" + YYYY + random 8 digits -> total length flexible
        public static string Generate(string bankCode = "B001", int randomDigits = 8)
        {
            var year = DateTime.UtcNow.Year.ToString();
            var rnd = new Random();
            var digits = string.Concat(Enumerable.Range(0, randomDigits).Select(_ => rnd.Next(0, 10).ToString()));
            return $"{bankCode}{year}{digits}";
        }
    }
}
