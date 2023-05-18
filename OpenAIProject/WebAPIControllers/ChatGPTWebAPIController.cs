namespace OpenAIProject.WebAPIControllers
{
    using DevExtreme.AspNet.Data;
    using DevExtreme.AspNet.Mvc;

    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using OpenAI_API;
    using OpenAIProject.Interfaces;
    using OpenAIProject.Models;
    using System.Net.Http.Headers;
    using static OpenAIProject.WebAPIControllers.ChatGPTWebAPIController;

    [Route("api/[controller]")]
    public class ChatGPTWebAPIController : Controller
    {
        private readonly IChatService chatService;

        public ChatGPTWebAPIController(IChatService chatService)
        {
            this.chatService = chatService;
        }

        [HttpGet("/GetChats")]
        public IActionResult GetChats(DataSourceLoadOptions loadOptions) 
        {
            var chats = new List<ChatGPTMessage>();
            chats = this.chatService.GetAll();
            return this.Json(DataSourceLoader.Load(chats, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            OpenAIAPI api = new OpenAIAPI("API_KEY_HERE");

            var model = new ChatGPTMessage();
            JsonConvert.PopulateObject(values, model);
            model.role = "user";

            var chat = api.Chat.CreateConversation();

            chat.AppendUserInput(model.content);

            string response = await chat.GetResponseFromChatbotAsync();

            var output = new ChatGPTMessage();
            output.role = "assistant";
            output.content = response;

            this.chatService.Add(model);

            this.chatService.Add(output);

            return Ok();
        }
    }
}
