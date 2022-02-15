using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Admin.Categories
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public Category CategoryObj { get; set; }
        public UpsertModel(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;


        public IActionResult OnGet(int? id)
        {
            CategoryObj = new Category();

            if(id != 0) //Edit Mode
            {
                CategoryObj = _unitOfWork.Category.Get(u => u.Id == id);
                if (CategoryObj == null)
                {
                    return NotFound();
                }
            }
            return Page(); //assume insert new mode
        }
        
        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            //If New
            if(CategoryObj.Id == 0)
            {
                _unitOfWork.Category.Add(CategoryObj);
            }

            //Existing
            else
            {
                _unitOfWork.Category.Update(CategoryObj);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
