using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreApp.Data.Abstract;
using StoreApp.Data.Concrete;
using StoreApp.Web.Models;

namespace StoreApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IWebHostEnvironment _webHostEnvironment; 

        public ProductController(IStoreRepository storeRepository, IWebHostEnvironment webHostEnvironment)
        {
            _storeRepository = storeRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        
        public IActionResult Index()
        {
            var model = _storeRepository.Products.ToList();
            return View(model);
        }

        
        public IActionResult Create()
        {
            ViewBag.Categories = _storeRepository.Categories.ToList();
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductViewModel model, IFormFile? file)
        {
            var extension="";
            if(file!=null)
            {
                extension=Path.GetExtension(file.FileName).ToLower();
                var allowedExtension=new[]{".jpg",".jpeg",".png"};
                if(!allowedExtension.Contains(extension))
                {
                    ModelState.AddModelError("","Gecerli bir resim şeçiniz");
                    return View(model);
                }
            }
    
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var randomFileName=string.Format($"{Guid.NewGuid()}{extension}");
                    var path=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",randomFileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    model.image = String.Concat("/img/",randomFileName);
                }
                Product productEntity=new Product()
                {
                    Name=model.Name,
                    Description=model.Description,
                    Price=(double)model.Price,
                    image=model.image,
                };
                var selectedCategories = _storeRepository.Categories
                                        .Where(c => model.SelectedCategoryIds.Contains(c.Id))
                                        .ToList();
                productEntity.Categories.AddRange(selectedCategories);
                _storeRepository.CreateBuilder(productEntity);

                return RedirectToAction("Index");
            }
            ViewBag.Categories = _storeRepository.Categories.ToList();
            return View(model);
        }
        public IActionResult Edit(int id)
        {
            var product = _storeRepository.GetOneProduct(id);

            if (product == null) return NotFound();

            var model = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = (decimal)product.Price,
                image = product.image,

                SelectedCategoryIds = product.Categories.Select(c => c.Id).ToArray()
            };

            ViewBag.Categories = _storeRepository.Categories.ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ProductViewModel model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                
                var entity = _storeRepository.GetOneProduct(model.Id);

                if (entity == null) return NotFound();


                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    var randomFileName = $"{Guid.NewGuid()}{extension}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    entity.image = $"/img/{randomFileName}";
                }
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Price = (double)model.Price;

                entity.Categories.Clear();

                var newCategories = _storeRepository.Categories
                    .Where(c => model.SelectedCategoryIds.Contains(c.Id))
                    .ToList();

                entity.Categories.AddRange(newCategories);

                _storeRepository.UpdateOneProduct(entity);

                return RedirectToAction("Index");
            }

            ViewBag.Categories = _storeRepository.Categories.ToList();
            return View(model);
        }
        public IActionResult Delete( int id)
        {
            var product = _storeRepository.GetOneProduct(id);

            if (product != null)
            {
                _storeRepository.DeleteOneProduct(id);
            }

            return RedirectToAction("Index");
        }
    }
}