using System.Globalization;

namespace SellBot.Wrappers
{
    internal class Utility
    {
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            DateTime epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var span = dateTime - epoch;
            return (long)span.TotalSeconds;
        }

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime epochDateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return epochDateTime.AddSeconds(unixTimeStamp);
        }

        public static decimal StringToDecimal(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return 0m;

            return decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result) ? result : 0m;
        }

        public static string DecimalToString(decimal value, string format = null)
        {
            return string.IsNullOrEmpty(format) ? value.ToString(CultureInfo.InvariantCulture) : value.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}
