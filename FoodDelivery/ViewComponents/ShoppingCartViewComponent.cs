using ApplicationCore.Interfaces;
using Infrastructure.Utilty;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodDelivery.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            int count = 0;
            if (claim != null)
            {
                if(HttpContext.Session.GetInt32(SD.ShoppingCart) != null)
                {
                    return View(HttpContext.Session.GetInt32(SD.ShoppingCart));
                }
                else
                {
                    count = _unitOfWork.ShoppingCart.List(u => u.ApplicationUserId == claim.Value).ToList().Count;
                    HttpContext.Session.SetInt32(SD.ShoppingCart, count);
                    return View(count);
                }
            }
            else
            {
                HttpContext.Session.Clear();
                return View(count);
            }
        }
    }
}
