using System;
using StoreApp.Data.Concrete;

namespace StoreApp.Data.Abstract;

public interface IOrderRepository
{
    IQueryable<Order>Orders();
    void SaveOrder(Order order);
}
