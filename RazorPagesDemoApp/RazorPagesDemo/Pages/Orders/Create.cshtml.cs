using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RazorPagesDemo.Pages.Orders
{
    public class Create : PageModel
    {
        private readonly IFoodData _foodData;
        private readonly IOrderData _orderData;

        public Create(IFoodData foodData, IOrderData orderData)
        {
            _foodData = foodData;
            _orderData = orderData;
        }

        public List<SelectListItem> FoodItems { get; set; }

        [BindProperty]
        public OrderModel Order { get; set; }

        public async Task OnGet()
        {
            var food = await _foodData.GetFood();

            FoodItems = new List<SelectListItem>();

            food.ForEach(x => { FoodItems.Add(new SelectListItem {Value = x.Id.ToString(), Text = x.Title}); });
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var food = await _foodData.GetFood();
            var orderedFood = food.First(f => f.Id == Order.FoodId);

            Order.OrderDate = DateTime.UtcNow;
            Order.Total = orderedFood.Price * Order.Quantity;

            var id = await _orderData.CreateOrder(Order);

            return RedirectToPage("./Create");
        }
    }
}
