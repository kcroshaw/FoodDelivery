using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Customer.Home
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitofWork;
        public IndexModel(IUnitOfWork unitofWork) => _unitofWork = unitofWork;

        public IEnumerable<MenuItem> MenuItemList { get; set; }
        public IEnumerable<Category> CategoryList { get; set; }

        public void OnGet()
        {
            MenuItemList = _unitofWork.MenuItem.List(null, null, "Category,FoodType");
            CategoryList = _unitofWork.Category.List(null, q => q.DisplayOrder, null);
        }
    }
}
