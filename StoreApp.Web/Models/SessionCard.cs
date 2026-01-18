using System.Text.Json.Serialization;
using StoreApp.Data.Concrete;
using StoreApp.Web.Helpers;

namespace StoreApp.Web.Models;

public class SessionCard:Cart
{
    public static Cart GetCart(IServiceProvider services)
    {
        ISession? session=services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
        SessionCard cart=session?.GetJson<SessionCard>("Cart")?? new SessionCard();
        cart.Session=session;
        return cart;
    }
    
    [JsonIgnore]
    public ISession? Session { get; set; }
    public override void AddItem(Product product, int quantity)
    {
        base.AddItem(product, quantity);
        Session?.SetJson("Cart",this);

    }
    public override void RemoveItem(Product product)
    {
        base.RemoveItem(product);
        Session?.SetJson("Cart", this);
    }
    public override void Clear()
    {
        base.Clear();
        Session?.Remove("Cart");
    }
}
