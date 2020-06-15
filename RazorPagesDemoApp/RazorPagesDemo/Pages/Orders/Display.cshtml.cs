using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesDemo.Models;

namespace RazorPagesDemo.Pages.Orders
{
    public class Display : PageModel
    {
        private readonly IOrderData _orderData;
        private readonly IFoodData _foodData;

        public Display(IOrderData orderData, IFoodData foodData)
        {
            _orderData = orderData;
            _foodData = foodData;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public OrderUpdateModel UpdateModel { get; set; }

        public OrderModel Order { get; set; }
        public string ItemPurchased { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Order = await _orderData.GetOrder(Id);

            if (Order != null)
            {
                var food = await _foodData.GetFood();
                ItemPurchased = food.FirstOrDefault(x => x.Id == Order.FoodId)?.Title;
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            await _orderData.UpdateOrderName(UpdateModel.Id, UpdateModel.OrderName);

            return RedirectToPage("./Display", new {UpdateModel.Id});
        }
    }
}
