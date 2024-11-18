using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Controllers;
using Models;
using Moq;
using Repositories;
using Xunit;

namespace Tests.Controllers
{
    public class OrdersControllerTests
    {
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _controller = new OrdersController(new OrderRepository());
        }

        [Fact]
        public void CreateOrder_ReturnsCreatedAtActionResult()
        {
            var order = new Order { Id = Guid.NewGuid(), Customer = "John Doe" };

            var result = _controller.CreateOrder(order);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetOrderById), createdAtActionResult.ActionName);
            Assert.Equal(order.Id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(order, createdAtActionResult.Value);
        }

        [Fact]
        public void GetOrderById_ReturnsOkObjectResult_WhenOrderExists()
        {
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId, Customer = "John Doe" };
            _controller.CreateOrder(order);
            var result = _controller.GetOrderById(orderId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(order, okResult.Value);
        }

        [Fact]
        public void GetOrderById_ReturnsNotFoundResult_WhenOrderDoesNotExist()
        {
            var orderId = Guid.NewGuid();

            var result = _controller.GetOrderById(orderId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetOrdersByCustomer_ReturnsOkObjectResult()
        {
            var customerName = "John Doe";
            var orders = new List<Order> { new Order { Id = Guid.NewGuid(), Customer = customerName } };
            _controller.CreateOrder(orders[0]);
            var result = _controller.GetOrdersByCustomer(customerName);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(orders, okResult.Value);
        }

        [Fact]
        public void UpdateOrder_ReturnsNoContentResult_WhenOrderIsUpdated()
        {
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId, Customer = "John Doe" };
            _controller.CreateOrder(order);
            var result = _controller.UpdateOrder(orderId, order);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateOrder_ReturnsBadRequestResult_WhenIdMismatch()
        {
            var orderId = Guid.NewGuid();
            var order = new Order { Id = Guid.NewGuid(), Customer = "John Doe" };
_controller.CreateOrder(order);
            var result = _controller.UpdateOrder(orderId, order);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void UpdateOrder_ReturnsNotFoundResult_WhenOrderDoesNotExist()
        {
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId, Customer = "John Doe" };
            var result = _controller.UpdateOrder(orderId, order);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void CancelOrder_ReturnsNoContentResult_WhenOrderIsCanceled()
        {
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId, Customer = "John Doe" };
            _controller.CreateOrder(order);
            var result = _controller.CancelOrder(orderId) as NoContentResult;

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void CancelOrder_ReturnsNotFoundResult_WhenOrderDoesNotExist()
        {
            var orderId = Guid.NewGuid();
            var result = _controller.CancelOrder(orderId);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
