using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data.Abstract;
using StoreApp.Data.Concrete;
using StoreApp.Web.Models;

namespace StoreApp.Web.Controllers;

public class HomeController:Controller
{
    public int pageSize=3;
    private readonly IStoreRepository _storeRepository;
    private readonly IMapper _mapper;
    public HomeController(IStoreRepository storeRepository, IMapper mapper)
    {
        _storeRepository= storeRepository;
        _mapper=mapper;
    }
    //./?page=1
    public IActionResult Index(string category, int page=1)
    {
        
          
        return View(new ProductsListViewModel
        {
            Products=_storeRepository.GetProductsByCategory(category,page,pageSize)
            .Select(product =>_mapper.Map<ProductViewModel>(product)),
            PageInfo= new PageInfo
            {
                ItemsPerPage=pageSize,
                CurrentPage = page,
                TotalItems =_storeRepository.GetProductCount(category)

            }
        });
    }
}