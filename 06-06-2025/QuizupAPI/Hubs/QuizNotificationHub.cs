using Microsoft.AspNetCore.SignalR;
using QuizupAPI.Models;
using QuizupAPI.Interfaces;
using System.Threading.Tasks;
using QuizupAPI.Models.DTOs.Notifications;

namespace QuizupAPI.Hubs
{
    public class QuizNotificationHub : Hub
    {
        public async Task NotifyStartQuiz(QuizNotificationDTO quiz)
        {
            await Clients.All.SendAsync("NotifyStartQuiz", quiz);
        }

        public async Task NotifyEndQuiz(QuizNotificationDTO quiz)
        {
            await Clients.All.SendAsync("NotifyEndQuiz", quiz);
        }
    }
}