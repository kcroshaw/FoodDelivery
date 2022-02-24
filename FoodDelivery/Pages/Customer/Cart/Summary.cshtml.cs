using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using FoodDelivery.ViewModels;
using Infrastructure.Utilty;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Customer.Cart
{
	public class SummaryModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public SummaryModel(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [BindProperty]
        public OrderDetailsCartVM OrderDetailsCart { get; set; }

        public void OnGet()
        {
            OrderDetailsCart = new OrderDetailsCartVM()
            {
                OrderHeader = new OrderHeader(),
                ListCart = new List<ShoppingCart>()
            };
            OrderDetailsCart.OrderHeader.OrderTotal = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim != null)
            {
                IEnumerable<ShoppingCart> cart = _unitOfWork.ShoppingCart.List(c => c.ApplicationUserId == claim.Value);
                if(cart != null)
                {
                    OrderDetailsCart.ListCart = cart.ToList();
                }
                foreach(var cartList in OrderDetailsCart.ListCart)
                {
                    cartList.MenuItem = _unitOfWork.MenuItem.Get(n => n.Id == cartList.MenuItemId);
                    OrderDetailsCart.OrderHeader.OrderTotal += (cartList.MenuItem.Price * cartList.Count);
                }
                OrderDetailsCart.OrderHeader.OrderTotal += OrderDetailsCart.OrderHeader.OrderTotal * SD.SalesTaxPercent;
                ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(c => c.Id == claim.Value);
                OrderDetailsCart.OrderHeader.DeliveryName = applicationUser.FullName;
                OrderDetailsCart.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
                OrderDetailsCart.OrderHeader.DeliveryTime = DateTime.Now;
                OrderDetailsCart.OrderHeader.DeliveryDate = DateTime.Now;
            }
        }
    }
}
