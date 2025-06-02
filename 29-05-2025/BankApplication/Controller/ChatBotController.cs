using Microsoft.AspNetCore.Mvc;
using BankApplication.Models;
using BankApplication.Models.DTO;
using BankApplication.Interfaces;
using System;

namespace BankApplication.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatBotController : ControllerBase
    {
        private readonly IChatBotService _chatBotService;
        public ChatBotController(IChatBotService chatBotService)
        {
            _chatBotService = chatBotService;
        }

        [HttpPost("ask-question")]
        public async Task<IActionResult> AskQuestion([FromBody] ChatBotQuestionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Question))
            {
                return BadRequest(new { message = "Question is required." });
            }

            var botResponseText = await _chatBotService.AskQuestionAsync(request.Question);

            if (string.IsNullOrWhiteSpace(botResponseText))
            {
                return BadRequest(new { message = "Unable to get a response from the bot." });
            }

            var responseMessage = new Message
            {
                Sender = "bot",
                Content = botResponseText,
                Timestamp = DateTime.UtcNow
            };

            return Ok(responseMessage);
        }
    }
}
