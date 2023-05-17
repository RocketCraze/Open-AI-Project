namespace OpenAIProject.WebAPIControllers
{
    using DevExtreme.AspNet.Data;
    using DevExtreme.AspNet.Mvc;

    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using OpenAIProject.Interfaces;
    using OpenAIProject.Models;
    using System.Net.Http.Headers;

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
            var model = new ChatGPTMessage();
            JsonConvert.PopulateObject(values, model);

            this.chatService.Add(model);

            var content = "{\r\n    \"model\": \"gpt-3.5-turbo\",\r\n    \"messages\": [\r\n        {\r\n            \"role\": \"user\",\r\n            \"content\": \"Hello\"\r\n        }\r\n    ]\r\n}";

            JObject jsonObject = JObject.Parse(content);

            string newContent = model.content;

            jsonObject["messages"][0]["content"] = newContent;

            var modifiedContent = jsonObject.ToString();

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://openai80.p.rapidapi.com/chat/completions"),
                Headers =
                {
                    { "X-RapidAPI-Key", "6ab0e14b9fmsh34afd798c835ac7p18f151jsn85c906eac155" },
                    { "X-RapidAPI-Host", "openai80.p.rapidapi.com" },
                },
                Content = new StringContent(modifiedContent)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);

            var output = new ChatGPTMessage();
            JsonConvert.PopulateObject(body, output);

            this.chatService.Add(output);

            return Ok();
        }

        public class Chat
        {
            public string model { get; set; }

            public ChatGPTMessage messages { get; set; }
        }
    }
}
