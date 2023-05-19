namespace OpenAIProject.WebAPIControllers
{
    using DevExtreme.AspNet.Data;
    using DevExtreme.AspNet.Mvc;

    using FluentValidation;
    using FluentValidation.AspNetCore;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    using Newtonsoft.Json;

    using OpenAI;
    using OpenAI.Edits;

    using OpenAIProject.Interfaces;
    using OpenAIProject.Models;

    [Route("api/[controller]")]
    public class DaVinciWebAPIController : Controller
    {
        private readonly IEditService editService;
        private readonly IValidator<DaVinciEdit> validator;

        public DaVinciWebAPIController(IEditService editService, IValidator<DaVinciEdit> validator)
        {
            this.editService = editService;
            this.validator = validator;
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

            var result = this.validator.Validate(model, _ => _.IncludeRuleSets("Create"));
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return this.BadRequest(this.ModelState.ToFullErrorString());
            }

            var request = new EditRequest(model.content, "Fix the spelling mistakes");
            var response = await api.EditsEndpoint.CreateEditAsync(request);

            if (response != null)
            {
                var output = new DaVinciEdit();
                output.role = "assistant";
                output.content = response.ToString();

                result = this.validator.Validate(output, _ => _.IncludeRuleSets("Create"));
                if (!result.IsValid)
                {
                    result.AddToModelState(this.ModelState);
                    return this.BadRequest(this.ModelState.ToFullErrorString());
                }

                this.editService.Add(model);

                this.editService.Add(output);
            }
            else
            {
                return this.BadRequest("Error retrieving information");
            }            

            return Ok();
        }
    }
}
