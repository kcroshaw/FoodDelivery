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
                MenuItem = await _unitOfWork.MenuItem.GetAsync(m => m.Id == id, false, "Category,FoodType")
            };

            ShoppingCartObj.MenuItemId = id;


        }

        public IActionResult OnPost()
        {
            if(ShoppingCartObj.ApplicationUserId == null)
            {
                return Redirect("~/Identity/Account/Login");
            }

            //get the applicationuserid from AspNetUsers table
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartObj.ApplicationUserId = claim.Value;

            //if there's already a cart for this user, retrieve it
            ShoppingCart cartFromDb = _unitOfWork.
                ShoppingCart.Get(c => c.ApplicationUserId == ShoppingCartObj.ApplicationUserId
                && c.MenuItemId == ShoppingCartObj.MenuItemId);

            if (cartFromDb == null)
            {
                _unitOfWork.ShoppingCart.Add(ShoppingCartObj);
            }
            else
            {
                cartFromDb.Count += ShoppingCartObj.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            _unitOfWork.Commit();

            //this is for our Icon on the shared layout menu (3)
            var count = _unitOfWork.ShoppingCart.List(c => c.ApplicationUserId == ShoppingCartObj.ApplicationUserId).Count();
            HttpContext.Session.SetInt32(SD.ShoppingCart, count);
            return RedirectToPage("./Index");


        }

    }
}