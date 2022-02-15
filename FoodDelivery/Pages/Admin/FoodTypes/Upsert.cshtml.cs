using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Admin.FoodTypes
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public FoodType FoodTypeObj { get; set; }
        public UpsertModel(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;


        public IActionResult OnGet(int? id)
        {
            FoodTypeObj = new FoodType();

            if (id != 0) //Edit Mode
            {
                FoodTypeObj = _unitOfWork.FoodType.Get(u => u.Id == id);
                if (FoodTypeObj == null)
                {
                    return NotFound();
                }
            }
            return Page(); //assume insert new mode
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //If New
            if (FoodTypeObj.Id == 0)
            {
                _unitOfWork.FoodType.Add(FoodTypeObj);
            }

            //Existing
            else
            {
                _unitOfWork.FoodType.Update(FoodTypeObj);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
