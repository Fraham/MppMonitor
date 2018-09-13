using Microsoft.AspNetCore.Mvc;

namespace MppMonitor.ViewComponents
{
    public class ArQueueSizeViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
