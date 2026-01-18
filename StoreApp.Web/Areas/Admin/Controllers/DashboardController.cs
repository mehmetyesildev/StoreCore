using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Data.Abstract;

namespace StoreApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IStoreRepository _storeRepository;

        public DashboardController(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public IActionResult Index()
        {
            
            ViewBag.ProductCount = _storeRepository.Products.Count();

            ViewBag.CategoryCount = _storeRepository.Categories.Count();

            ViewBag.TotalInventoryValue = _storeRepository.Products.Sum(p => (double?)p.Price) ?? 0;

            return View();
        }
    }
}