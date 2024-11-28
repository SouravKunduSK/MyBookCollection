using Microsoft.AspNetCore.Mvc;

namespace MyBookCollection.Controllers
{
    public class Authentication : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
