using System;
using System.Linq;
using System.Threading.Tasks;
using ApiDemoApp.Models;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiDemoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IFoodData _foodData;
        private readonly IOrderData _orderData;

        public OrderController(IFoodData foodData, IOrderData orderData)
        {
            _foodData = foodData;
            _orderData = orderData;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return NoContent();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var order = await _orderData.GetOrder(id);

            if (order != null)
            {
                var food = await _foodData.GetFood();
                order.Total = await CalculateTotal(order);

                var output = new
                {
                    Order = order,
                    ItemPurchased = food.FirstOrDefault(x => x.Id == order.FoodId)?.Title
                };

                return Ok(output);
            }

            return NotFound(new {Message = "No item found"});
        }

        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(OrderModel order)
        {
            order.Total = await CalculateTotal(order);

            var id = await _orderData.CreateOrder(order);

            return Ok(new {Id = id});
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] OrderUpdateModel order)
        {
            await _orderData.UpdateOrderName(order.Id, order.OrderName);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderData.DeleteOrder(id);
            return Ok();
        }

        public async Task<decimal> CalculateTotal(OrderModel order)
        {
            var food = await _foodData.GetFood();
            var price = food.Find(x => x.Id == order.FoodId)!.Price;
            return price * order.Quantity;
        }
    }
}
