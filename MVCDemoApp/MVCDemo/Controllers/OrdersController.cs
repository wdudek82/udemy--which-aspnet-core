using System.Linq;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCDemo.Models;

namespace MVCDemo.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IFoodData _foodData;
        private readonly IOrderData _orderData;

        public OrdersController(IFoodData foodData, IOrderData orderData)
        {
            _foodData = foodData;
            _orderData = orderData;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var food = await _foodData.GetFood();

            var model = new OrderCreateModel();

            food.ForEach(x => { model.FoodItems.Add(new SelectListItem {Value = x.Id.ToString(), Text = x.Title}); });

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderModel order)
        {
            if (ModelState.IsValid == false)
            {
                return View();
            }

            var food = await _foodData.GetFood();
            var price = food.Find(x => x.Id == order.FoodId)!.Price;

            order.Total = order.Quantity * price;

            var id = await _orderData.CreateOrder(order);

            return RedirectToAction("Display", new { id });
        }

        public async Task<IActionResult> Display(int id)
        {
            var displayOrder = new OrderDisplayModel
            {
                Order = await _orderData.GetOrder(id)
            };

            if (displayOrder.Order != null)
            {
                var food = await _foodData.GetFood();

                displayOrder.ItemPurchased = food.FirstOrDefault(x => x.Id == displayOrder.Order.FoodId)?.Title;
            }

            return View(displayOrder);
        }
    }
}
