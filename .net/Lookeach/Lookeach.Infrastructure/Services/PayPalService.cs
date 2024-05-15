using Lookeach.Application.DTOs;
using Lookeach.Application.Exceptions;
using Lookeach.Application.Interfaces.Services;
using Lookeach.Application.Paypals.Models;
using Lookeach.Core.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace Lookeach.Infrastructure.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly PayPalKeyDTO _paypalKey = new PayPalKeyDTO();
        private readonly IConfiguration _configuration;
        private const string BASE_ADDRESS = "https://api-m.sandbox.paypal.com";
        public PayPalService(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.GetSection(nameof(PayPalKeyDTO)).Bind(_paypalKey);
        }

        public async Task<string> CreateOrderPayPal(PayPalOrderModel payPalOrder)
        {
            if (payPalOrder == null!) throw new PayPalException("PayPal Order null !!!");
            using var client = new HttpClient();
            client.BaseAddress = new Uri(BASE_ADDRESS);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var authenticationString = $"{_paypalKey.ClientId}:{_paypalKey.ClientSecret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

            var json = JsonConvert.SerializeObject(payPalOrder);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/v2/checkout/orders");
            requestMessage.Content = new StringContent(json);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            requestMessage.Headers.Add("Prefer", "return=representation");
            try
            {
                var response = await client.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(responseBody)) throw new PayPalException("Response body null!!!");
                    return responseBody;
                }
                return null!;
            }
            catch (HttpRequestException http)
            {
                throw new PayPalException(http.HttpRequestError.ToString() + ", " + http.Message);
            }
        }

        public async Task<string> CompletedOrder(string token)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var authenticationString = $"{_paypalKey.ClientId}:{_paypalKey.ClientSecret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"/v2/checkout/orders/{token}/capture");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                requestMessage.Headers.Add("Prefer", "return=representation");
                requestMessage.Content = new StringContent("{}");
                requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.SendAsync(requestMessage);
                string responseBody = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
                return responseBody;
            }
            catch (HttpRequestException http)
            {
                throw new PayPalException(http.HttpRequestError.ToString() + ", " + http.Message);
            }
        }
    }
}
