using E_commerce.Core.Dtos;
using E_commerce.Core.Entities;
using E_commerce.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;


namespace E_commerce.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public PaymentService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<BaseResponse<string>> InitiatePaymentAsync(Order order)
        {
            var paystackSecret = _config["Paystack:SecretKey"]; // Store in appsettings
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", paystackSecret);

            var callbackUrl = _config["Paystack:CallbackUrl"];

            var paymentRequest = new
            {
                amount = (int)(order.TotalPrice * 100), // Paystack uses kobo
                email = order.Customer?.Email ?? "cavking@yopmail.com", // Fetch actual email from user
                reference = order.Name,
                callback_url = callbackUrl
            };

            var content = new StringContent(JsonConvert.SerializeObject(paymentRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://api.paystack.co/transaction/initialize", content);

            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new BaseResponse<string>
                {
                    Status = false,
                    Message = "Payment initiation failed",
                    Data = null
                };
            }

            dynamic result = JsonConvert.DeserializeObject(responseString);
            string authorizationUrl = result.data.authorization_url;

            return new BaseResponse<string>
            {
                Status = true,
                Message = "Payment link generated successfully",
                Data = authorizationUrl
            };
        }
    }

}
