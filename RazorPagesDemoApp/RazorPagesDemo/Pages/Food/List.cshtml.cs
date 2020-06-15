using System.Collections.Generic;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesDemo.Pages.Food
{
    public class List : PageModel
    {
        private readonly IFoodData _foodData;

        public List(IFoodData foodData)
        {
            _foodData = foodData;
        }

        public List<FoodModel> Food { get; set; }
        public async Task OnGet()
        {
            Food = await _foodData.GetFood();
        }
    }
}
