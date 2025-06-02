using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BankApplication.Models;
using BankApplication.Interfaces;
using Newtonsoft.Json;

namespace BankApplication.Services
{
    public class ChatBotService : IChatBotService
    {
        private static readonly HttpClient _httpClient = new HttpClient();


        public async Task<string> AskQuestionAsync(string question)
        {
            var requestBody = new { question = question };
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://127.0.0.1:5000/chat", content);

            if (!response.IsSuccessStatusCode)
            {
                return "Sorry, I couldn't get a response from the bot server.";
            }

            var responseString = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseString);
            return result.answer != null ? (string)result.answer : "No answer from bot.";
        }

        
    }
}