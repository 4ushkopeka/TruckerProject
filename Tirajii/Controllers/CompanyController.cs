using Microsoft.AspNetCore.Mvc;

namespace Tirajii.Controllers
{
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
