namespace OpenAIProject.WebAPIControllers
{
    using DevExtreme.AspNet.Data;
    using DevExtreme.AspNet.Mvc;

    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    using OpenAI_API;
    using OpenAI_API.Chat;
    using OpenAI_API.Models;

    using OpenAIProject.Interfaces;
    using OpenAIProject.Models;

    [Route("api/[controller]")]
    public class DaVinciWebAPIController : Controller
    {
        private readonly IEditService editService;

        public DaVinciWebAPIController(IEditService editService)
        {
            this.editService = editService;
        }

        [HttpGet("/GetEdits")]
        public IActionResult GetEdits(DataSourceLoadOptions loadOptions)
        {
            var chats = new List<DaVinciEdit>();
            chats = this.editService.GetAll();
            return this.Json(DataSourceLoader.Load(chats, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            OpenAIAPI api = new OpenAIAPI("YOUR_API_KEY_HERE");

            var model = new DaVinciEdit();
            JsonConvert.PopulateObject(values, model);
            model.role = "user";

            var chat = api.Chat.CreateConversation();

            chat.AppendUserInput(model.content);

            string response = await chat.GetResponseFromChatbotAsync();

        //    var result = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        //    {
        //        Model = Model.DavinciText,
        //        Temperature = 0.1,
        //        MaxTokens = 50,
        //        Instructions = "Fix the spelling mistakes",
        //        Messages = new ChatMessage[] {
        //    new ChatMessage(ChatMessageRole.User, "Hello!")
        //}
        //    });

            var output = new DaVinciEdit();
            output.role = "assistant";
            output.content = response;

            this.editService.Add(model);

            this.editService.Add(output);

            return Ok();
        }
    }
}
