using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tirajii.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        public IActionResult OfferMine()
        {
            return View();
        }
        public IActionResult OfferAdd()
        {
            return View();
        }
        public IActionResult OfferEdit()
        {
            return View();
        }
    }
}
