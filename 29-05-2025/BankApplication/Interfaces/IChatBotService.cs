using System.Collections.Generic;
using BankApplication.Models;

namespace BankApplication.Interfaces
{
    public interface IChatBotService
    {
        Task<string> AskQuestionAsync(string question);
    }
}