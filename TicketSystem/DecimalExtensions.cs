using System.Globalization;

namespace TicketSystem
{
    public static class DecimalExtensions
    {
        private static readonly CultureInfo ThaiCultureInfo = new CultureInfo("th-TH");

        public static string ToThaiCurrencyDisplayString(this decimal value) => value.ToString("C", ThaiCultureInfo);
    }
}
