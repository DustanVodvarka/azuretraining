using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAppFoodOrder.Services;
using WebAppFoodOrder.Services.Models;

namespace WebAppFoodOrder.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class OrderFunctionController
    {
        private static OrderService _orderService;

        public OrderFunctionController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("order")]
        public async Task<IActionResult> GetActiveOrders()
        {
            var result = await _orderService.GetActiveOrders();
            return new OkObjectResult(result);
        }

        [HttpGet("order/complete")]
        public async Task<IActionResult> GetCompletedOrders()
        {
            var result = await _orderService.GetCompletedOrders();
            return new OkObjectResult(result);
        }

        [HttpPost("order/{id}/complete")]
        public async Task<IActionResult> CompleteOrder(string id)
        {
            await _orderService.CompleteOrder(id);
            return new OkResult();
        }

        [HttpPost("order")]
        public async Task<IActionResult> SaveOrder(Order request)
        {
            Order result;
            if (request.OrderItems == null)
            {
                result = await _orderService.SaveRandomOrder(request.Name);
            }
            else
            {
                result = await _orderService.SaveOrder(request);
            }
            return new OkObjectResult(result);
        }
    }
}
