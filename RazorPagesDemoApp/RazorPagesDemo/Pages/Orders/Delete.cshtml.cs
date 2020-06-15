using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp;

namespace RazorPagesDemo.Pages.Orders
{
    public class Delete : PageModel
    {
        private readonly IOrderData _orderData;

        public Delete(IOrderData orderData)
        {
            _orderData = orderData;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public OrderModel Order { get; set; }

        public async Task OnGet()
        {
            Order = await _orderData.GetOrder(Id);
        }

        public async Task<IActionResult> OnPost()
        {
            await _orderData.DeleteOrder(Id);

            return RedirectToPage("./Create");
        }
    }
}
