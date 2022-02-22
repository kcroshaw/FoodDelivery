using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.Utilty;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace FoodDelivery.Pages.Customer.Home
{
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public DetailsModel(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [BindProperty]
        public ShoppingCart ShoppingCartObj { get; set; }

        public async Task OnGet(int id)
        {
            ShoppingCartObj = new ShoppingCart()
            {
                MenuItem = await _unitOfWork.MenuItem.GetAsync(m => m.Id == id, false, "C")
            };
            ShoppingCartObj.MenuItemId = id;
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                ShoppingCartObj.ApplicationUserId = claim.Value;

                ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(c => c.ApplicationUserId == ShoppingCartObj.ApplicationUserId && c.MenuItemId == ShoppingCartObj.MenuItem.Id);

                if(cartFromDb == null)
                {
                    _unitOfWork.ShoppingCart.Add(ShoppingCartObj);
                }
                else
                {
                    cartFromDb.Count += ShoppingCartObj.Count;
                    _unitOfWork.ShoppingCart.Update(cartFromDb);
                }
                _unitOfWork.Commit();

                var count = _unitOfWork.ShoppingCart.List(c => c.ApplicationUserId == ShoppingCartObj.ApplicationUserId).Count();
                HttpContext.Session.SetInt32(SD.ShoppingCart, count);
                return RedirectToPage("Index");
            }

            else
            {
                ShoppingCartObj.MenuItem = _unitOfWork.MenuItem.Get(m => m.Id == ShoppingCartObj.MenuItemId, false, "Category,FoodType");
            }
            return Page();
        }
    }
}
