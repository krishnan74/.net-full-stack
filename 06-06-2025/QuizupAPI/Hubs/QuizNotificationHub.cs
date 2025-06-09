using Microsoft.AspNetCore.SignalR;
using QuizupAPI.Models;
using QuizupAPI.Interfaces;
using System.Threading.Tasks;

namespace QuizupAPI.Hubs
{
    public class QuizNotificationHub : Hub
    {
        public async Task NotifyStartQuiz(Quiz quiz)
        {
            await Clients.All.SendAsync("NotifyStartQuiz", quiz);
        }

        public async Task NotifyEndQuiz(Quiz quiz)
        {
            await Clients.All.SendAsync("NotifyEndQuiz", quiz);
        }
    }
}