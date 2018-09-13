using Microsoft.AspNetCore.Mvc;
using MppMonitor.ViewModel;

namespace MppMonitor.Controllers
{
    public class PaymentAuthRateController : Controller
    {
        public IActionResult Index(string paymentProvider)
        {
            return View(new PaymentProviderViewModel { PaymentProvider = paymentProvider ?? "Overall" });
        }
    }
}