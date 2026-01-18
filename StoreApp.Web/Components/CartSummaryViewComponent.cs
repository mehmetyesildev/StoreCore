using Microsoft.AspNetCore.Mvc;
using StoreApp.Web.Models;

namespace StoreApp.Web.Companents;

public class CartSummaryViewComponent:ViewComponent
{
    private Cart cart;
    public CartSummaryViewComponent(Cart cartService)
    {
        cart=cartService;
    }
    public IViewComponentResult Invoke()
    {
        return View(cart);
    }
}
