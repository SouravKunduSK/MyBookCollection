using Microsoft.AspNetCore.Mvc;

namespace MyBookCollection.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
