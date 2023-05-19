namespace OpenAIProject.WebAPIControllers
{
    using System;

    using DevExtreme.AspNet.Mvc;

    using FluentValidation;
    using FluentValidation.AspNetCore;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    using Newtonsoft.Json;

    using OpenAI_API;
    using OpenAI_API.Images;

    using OpenAIProject.Interfaces;
    using OpenAIProject.Models;

    [Route("api/[controller]")]
    public class ImageGenerationWebAPIController : Controller
    {
        private readonly IImageService imageService;
        private readonly IValidator<ImageGenerationAI> validator;

        public ImageGenerationWebAPIController(IImageService imageService, IValidator<ImageGenerationAI> validator)
        {
            this.imageService = imageService;
            this.validator = validator;
        }

        public IActionResult Index(DataSourceLoadOptions loadOptions)
        {
            var models = new List<ImageGenerationAI>();
            models = this.imageService.GetAll();

            ViewBag.Message = models;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            var model = new ImageGenerationAI();
            JsonConvert.PopulateObject(values, model);
            var prompt = model.Prompt;

            var result = this.validator.Validate(model, _ => _.IncludeRuleSets("Create"));
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return this.BadRequest(this.ModelState.ToFullErrorString());
            }

            OpenAIAPI api = new OpenAIAPI("YOUR_API_KEY_HERE");

            var response = await api.ImageGenerations.CreateImageAsync(new ImageGenerationRequest(prompt, 1, ImageSize._256));

            if (response != null)
            {
                model.Image = response.Data[0].Url;

                using (var client = new HttpClient())
                {
                    var bytes = await client.GetByteArrayAsync(model.Image);
                    var base64String = Convert.ToBase64String(bytes);
                    model.Image = base64String;
                }

                this.imageService.Add(model);
            }
            else
            {
                return this.BadRequest("Error retrieving information");
            }

            return RedirectToAction("Index");
        }
    }
}