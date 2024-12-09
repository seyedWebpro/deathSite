using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace api.Services
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;

    public EmailService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SendEmailAsync(string apiKey, string to, string subject, string body)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.najva.com/send-email");
        request.Headers.Add("Authorization", $"Bearer {apiKey}");

        var content = new
        {
            to = to,
            subject = subject,
            body = body
        };

        request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
    }
}