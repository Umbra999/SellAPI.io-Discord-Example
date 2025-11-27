using System.Text;
using System.Text.Json;
using SellBot.Wrappers;

namespace SellBot
{
    internal class SellAPI
    {
        private readonly HttpClient _client;

        public SellAPI()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "SellBot");
            _client.BaseAddress = new Uri("https://sellapi.io/");
        }

        public async Task<Invoice> GetInvoice(string id)
        {
            var response = await _client.GetAsync($"/invoice/{id}");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Invoice>(json);
        }

        public async Task<Invoice> CreateCryptoInvoice(PaymentMethod method, string callbackUrl, decimal priceUsd, string addressOut, string customData = null)
        {
            var payload = new
            {
                method = (byte)method,
                callbackUrl = callbackUrl,
                price_usd = priceUsd,
                customData = customData,
                payout_address = addressOut
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/invoice", content);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError(await response.Content.ReadAsStringAsync());
                return null;
            }

            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Invoice>(body);
        }

        public async Task<Invoice> CreateRewarbleInvoice(PaymentMethod method, string callbackUrl, decimal priceUsd, string apiKey, string customData = null)
        {
            var payload = new
            {
                method = (byte)method,
                callbackUrl = callbackUrl,
                price = priceUsd,
                customData = customData,
                apiKey = apiKey
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/invoice", content);
            if (!response.IsSuccessStatusCode) return null;

            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Invoice>(body);
        }

        public enum PaymentStatus : byte
        {
            pending,
            partially_paid,
            paid
        }

        public enum PaymentMethod : byte
        {
            bitcoin = 0,
            litecoin = 1,
            ethereum = 2,
            solana = 3,
            doge = 4,
            tron = 5,
            rewarble = 6
        }

        public class Invoice
        {
            public string id { get; set; }
            public PaymentStatus status { get; set; }
            public PaymentMethod method { get; set; }
            public string callbackUrl { get; set; }
            public decimal price_usd_display { get; set; }
            public long expiration { get; set; }
            public string customData { get; set; }
            public object details { get; set; }
        }

        public class RewarbleInvoiceDetails
        {
            public string provided_codes { get; set; }
            public string apiKey { get; set; }
            public decimal price_usd { get; set; }
            public decimal paid_usd { get; set; }
        }

        public class CryptoInvoiceDetails
        {
            public string payment_address { get; set; }
            public string payout_address { get; set; }
            public decimal price_in_currency { get; set; }
            public decimal paid_in_currency { get; set; }
        }
    }
}
