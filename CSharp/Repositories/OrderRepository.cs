using System;
using System.Collections.Generic;
using System.Linq;
using Models;

#nullable enable
namespace Repositories;
public class OrderRepository 
{
    private readonly Dictionary<Guid, Order> _orders = new();
    public IEnumerable<Order> GetAll() => _orders.Values.AsEnumerable<Order>();
    public Order? GetById(Guid id) => _orders.TryGetValue(id, out var order) ? order : null;
    public Guid Add(Order order)
    {
        _orders.Add(order.Id, order);
        return order.Id;
    }
    public Order? Update(Order order)
    {
        if (_orders.ContainsKey(order.Id))
        {
            var orderToUpdate = _orders[order.Id];
            orderToUpdate.Customer = order.Customer;
            orderToUpdate.Product = order.Product;
            orderToUpdate.Quantity = order.Quantity;
            orderToUpdate.OrderDate = order.OrderDate;
            orderToUpdate.Canceled = order.Canceled;
            return order;
        }
        return null;
    }
    public bool Cancel(Guid id)
    {
        if (_orders.ContainsKey(id))
        {
            var orderToUpdate = _orders[id];
            orderToUpdate.Canceled = true;
            return true;
        }
        return false;
    }
}
