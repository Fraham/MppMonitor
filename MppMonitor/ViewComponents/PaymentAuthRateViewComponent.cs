using Microsoft.AspNetCore.Mvc;
using MppMonitor.ViewModel;

namespace MppMonitor.ViewComponents
{
    public class PaymentAuthRateViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string paymentProvider)
        {
            return View(new PaymentProviderViewModel { PaymentProvider = paymentProvider ?? "Overall" });
        }
    }
}
