namespace OpenAIProject.Controllers 
{
    using Microsoft.AspNetCore.Mvc;

    public class ChatGPTController : Controller {

        public IActionResult Index()
        {
            return View();
        }

    }
}