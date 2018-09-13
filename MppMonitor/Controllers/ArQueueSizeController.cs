using Microsoft.AspNetCore.Mvc;

namespace MppMonitor.Controllers
{
    public class ArQueueSizeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}