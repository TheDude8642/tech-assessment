// Controllers/OrdersController.cs
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderRepository _repository;

    public OrdersController(OrderRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public IActionResult CreateOrder(Order order)
    {
        _repository.Add(order);
        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    }

    [HttpGet("{id}")]
    public IActionResult GetOrderById(Guid id)
    {
        var order = _repository.GetById(id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpGet("customer/{customerName}")]
    public IActionResult GetOrdersByCustomer(string customer)
    {
        var orders = _repository.GetAll().Where(o => o.Customer == customer);
        return Ok(orders);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateOrder(Guid id, Order order)
    {
        if (id != order.Id)
        {
            return BadRequest();
        }

        var existingOrder = _repository.GetById(id);
        if (existingOrder == null)
        {
            return NotFound();
        }

        _repository.Update(order);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult CancelOrder(Guid id)
    {
        var order = _repository.GetById(id);
        if (order == null)
        {
            return NotFound();
        }

        _repository.Cancel(id);
        return NoContent();
    }
}
