namespace OpenAIProject.WebAPIControllers
{
    using DevExtreme.AspNet.Data;
    using DevExtreme.AspNet.Mvc;

    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;
    using OpenAI;
    using OpenAI.Edits;

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
            var api = new OpenAIClient("YOUR_API_KEY_HERE");

            var model = new DaVinciEdit();
            JsonConvert.PopulateObject(values, model);
            model.role = "user";

            var request = new EditRequest(model.content, "Fix the spelling mistakes");
            var response = await api.EditsEndpoint.CreateEditAsync(request);

            if (response != null)
            {
                var output = new DaVinciEdit();
                output.role = "assistant";
                Console.WriteLine(response);
                output.content = response.ToString();

                this.editService.Add(model);

                this.editService.Add(output);
            }
            else
            {
                return this.BadRequest();
            }

            

            return Ok();
        }
    }
}
