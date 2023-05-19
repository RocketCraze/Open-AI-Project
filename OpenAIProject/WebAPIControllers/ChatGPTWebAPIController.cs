namespace OpenAIProject.WebAPIControllers
{
    using DevExtreme.AspNet.Data;
    using DevExtreme.AspNet.Mvc;

    using FluentValidation;
    using FluentValidation.AspNetCore;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    using Newtonsoft.Json;

    using OpenAI_API;

    using OpenAIProject.Interfaces;
    using OpenAIProject.Models;

    [Route("api/[controller]")]
    public class ChatGPTWebAPIController : Controller
    {
        private readonly IChatService chatService;
        private readonly IValidator<ChatGPTMessage> validator;

        public ChatGPTWebAPIController(IChatService chatService, IValidator<ChatGPTMessage> validator)
        {
            this.chatService = chatService;
            this.validator = validator;
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
            OpenAIAPI api = new OpenAIAPI("YOUR_API_KEY_HERE");

            var model = new ChatGPTMessage();
            JsonConvert.PopulateObject(values, model);
            model.role = "user";

            var result = this.validator.Validate(model, _ => _.IncludeRuleSets("Create"));
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return this.BadRequest(this.ModelState.ToFullErrorString());
            }

            var chat = api.Chat.CreateConversation();

            chat.AppendUserInput(model.content);

            string response = await chat.GetResponseFromChatbotAsync();

            if (response != null)
            {
                var output = new ChatGPTMessage();
                output.role = "assistant";
                output.content = response;



                this.chatService.Add(model);

                this.chatService.Add(output);
            }
            else
            {
                return this.BadRequest("Error retrieving information");
            }            

            return Ok();
        }
    }
}
