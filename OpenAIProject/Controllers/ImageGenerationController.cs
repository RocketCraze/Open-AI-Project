using Microsoft.AspNetCore.Mvc;
using OpenAIProject.Models;

namespace OpenAIProject.Controllers
{
    public class ImageGenerationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
