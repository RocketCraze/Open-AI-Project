namespace OpenAIProject.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class DaVinciController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
