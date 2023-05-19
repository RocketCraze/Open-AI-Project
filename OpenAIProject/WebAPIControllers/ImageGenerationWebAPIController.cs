namespace OpenAIProject.WebAPIControllers
{
    using DevExtreme.AspNet.Data;
    using DevExtreme.AspNet.Mvc;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
    using Newtonsoft.Json;

    using OpenAI_API;
    using OpenAI_API.Images;

    using OpenAIProject.Interfaces;
    using OpenAIProject.Models;
    using System;

    [Route("api/[controller]")]
    public class ImageGenerationWebAPIController : Controller
    {
        private readonly IImageService imageService;

        public ImageGenerationWebAPIController(IImageService imageService)
        {
            this.imageService = imageService;
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

            OpenAIAPI api = new OpenAIAPI("YOUR_API_KEY_HERE");

            var result = await api.ImageGenerations.CreateImageAsync(new ImageGenerationRequest(prompt, 1, ImageSize._256));

            model.Image = result.Data[0].Url;

            using (var client = new HttpClient())
            {
                var bytes = await client.GetByteArrayAsync(model.Image);
                var base64String = Convert.ToBase64String(bytes);
                model.Image = base64String;
            }

            this.imageService.Add(model);

            return RedirectToAction("Index");
        }
    }
}
