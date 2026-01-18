
using System.Threading.Tasks;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Data.Abstract;
using StoreApp.Data.Concrete;
using StoreApp.Web.Models;

namespace StoreApp.Web.Controllers;

public class OrderController:Controller
{
    private Cart cart;
    private IOrderRepository _orderRepository;
    private UserManager<AppUser> _userManager;
    public OrderController(Cart cartService, IOrderRepository orderRepository, UserManager<AppUser> userManager)
    {
        _orderRepository=orderRepository;
        _userManager=userManager;
        cart=cartService;
    }
    public async Task<IActionResult> Checkout()
    {
        if (cart.Items.Count() == 0)
        {
            ModelState.AddModelError("", "Sepetinizde ürün yok.");
            return RedirectToAction("Index", "Home");
        }

        var model = new OrderModel() { Cart = cart };

        if (User.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name!);
            if (user != null)
            {
                model.Name = $"{user.FirstName} {user.LastName}";
                model.Email = user.Email!;
                model.City = user.City ?? "";
                model.AddressLine = user.Address ?? "";
            }
        }

        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Checkout(OrderModel model)
    {
        if(cart.Items.Count()==0)
        {
            ModelState.AddModelError("","Sepetinizde ürün yok");
        }
        if(ModelState.IsValid)
        {
            var order=new Order
            {
              Name=model.Name,  
              Email=model.Email,
              City=model.City,
              Phone=model.Phone,
              AddressLine=model.AddressLine,
              OrderDate=DateTime.Now,
              OrderItems=cart.Items.Select(i=>new StoreApp.Data.Concrete.OrderItem
              {
                  ProductId=i.Product.Id,
                  Price=(double)i.Product.Price,
                  Quantity=i.Quantity
              }).ToList()
            };
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    order.UserId = user.Id; 
                }
            }
            model.Cart=cart;
            var payment= ProcessPayment(model);
            if (payment.Status == "success")
            {
                _orderRepository.SaveOrder(order);
                cart.Clear();
                return RedirectToPage("/Compledet", new { OrderId = order.Id });
            }
            model.Cart = cart;
            return View(model);

        }
        else
        {
            model.Cart=cart;
            return View(model);
        }
    }

    private Payment ProcessPayment(OrderModel model)
    {
        Options options = new Options();
        options.ApiKey = "KendiApıkey";
        options.SecretKey = "Kendiseckey";
        options.BaseUrl = "https://sandbox-api.iyzipay.com";

        CreatePaymentRequest request = new CreatePaymentRequest();
        request.Locale = Locale.TR.ToString();
        request.ConversationId =new Random().Next(111111111,999999999).ToString();
        request.Price =model?.Cart?.CalculateTotal().ToString();
        request.PaidPrice = model?.Cart?.CalculateTotal().ToString(); 
        request.Currency = Currency.TRY.ToString();
        request.Installment = 1;
        request.BasketId = "B67832";
        request.PaymentChannel = PaymentChannel.WEB.ToString();
        request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

        PaymentCard paymentCard = new PaymentCard();
        paymentCard.CardHolderName = model?.CartName;
        paymentCard.CardNumber = model?.CartNumber;
        paymentCard.ExpireMonth = model?.ExpiretionMonth; ;
        paymentCard.ExpireYear = model?.ExpiretionYear;
        paymentCard.Cvc = model?.Cvc;
        paymentCard.RegisterCard = 0;
        request.PaymentCard = paymentCard;

        Buyer buyer = new Buyer();
        buyer.Id = "USR-"+new Random().Next(1000,9999);
        buyer.Name = model?.Name;
        buyer.Surname = "Müsteri";
        buyer.GsmNumber = model!.Phone;
        buyer.Email = model.Email;
        buyer.IdentityNumber = "11111111111";
        buyer.LastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        buyer.RegistrationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        buyer.RegistrationAddress = model.AddressLine;
        buyer.Ip = "85.34.78.112";
        buyer.City = model.City;
        buyer.Country = "Turkey";
        buyer.ZipCode = "34732";
        request.Buyer = buyer;

        Address shippingAddress = new Address();
        shippingAddress.ContactName = model.Name;
        shippingAddress.City = model.City;
        shippingAddress.Country = "Turkey";
        shippingAddress.Description = model.AddressLine;
        shippingAddress.ZipCode = "34742";
        request.ShippingAddress = shippingAddress;

        Address billingAddress = new Address();
        billingAddress.ContactName = model.Name;
        billingAddress.City = model.City;
        billingAddress.Country = "Turkey";
        billingAddress.Description = model.AddressLine;
        billingAddress.ZipCode = "34742";
        request.BillingAddress = billingAddress;

        List<BasketItem> basketItems = new List<BasketItem>();

        foreach (var item in model?.Cart?.Items ?? Enumerable.Empty<CartItem>() )
        {
            
            BasketItem firstBasketItem = new BasketItem();
            firstBasketItem.Id = item.Product.Id.ToString();;
            firstBasketItem.Name = item.Product.Name;
            firstBasketItem.Category1 = item.Product.Categories?.FirstOrDefault()?.Name ?? "Genel";
            firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            firstBasketItem.Price = item.Product.Price.ToString();
            basketItems.Add(firstBasketItem);
       
        }
        request.BasketItems = basketItems;

        Payment payment = Payment.Create(request, options).Result;
        return payment;
    }
}
