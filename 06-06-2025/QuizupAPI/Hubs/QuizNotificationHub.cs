using Microsoft.AspNetCore.SignalR;
using QuizupAPI.Models;
using QuizupAPI.Interfaces;
using System.Threading.Tasks;
using QuizupAPI.Models.DTOs.Notifications;

namespace QuizupAPI.Hubs
{
    public class QuizNotificationHub : Hub
    {
        public async Task JoinClassGroup(string classGroupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, classGroupName);
        }

        public async Task LeaveClassGroup(string classGroupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, classGroupName);
        }

        public async Task NotifyStartQuizToClass(string classGroupName, QuizNotificationDTO quiz)
        {
            await Clients.Group(classGroupName).SendAsync("NotifyStartQuiz", quiz);
        }

        public async Task NotifyEndQuizToClass(string classGroupName, QuizNotificationDTO quiz)
        {
            await Clients.Group(classGroupName).SendAsync("NotifyEndQuiz", quiz);
        }

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