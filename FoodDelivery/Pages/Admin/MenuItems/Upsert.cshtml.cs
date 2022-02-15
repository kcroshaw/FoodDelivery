using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodDelivery.Pages.Admin.MenuItems
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfwork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public MenuItem MenuItem { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> FoodTypeList { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfwork = unitOfWork;
            _webHostEnvironment = hostEnvironment;
        }
        public void OnGet(int? id)
        {
            if (id != null)
            {
                MenuItem = _unitOfwork.MenuItem.Get(u => u.Id == id, true);

                var categories = _unitOfwork.Category.List();
                var foodTypes = _unitOfwork.FoodType.List();

                CategoryList = categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
                FoodTypeList = foodTypes.Select(f => new SelectListItem { Text = f.Name, Value = f.Id.ToString() });
            }

            if (id == null)
            {
                MenuItem = new();
            }
        }

        public IActionResult OnPost(int? id)
        {

            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (MenuItem.Id == 0)
            {
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"iimages\menuitems\");
                    var extension = Path.GetExtension(files[0].FileName);

                    var fullPath = uploads + fileName + extension;
                    using var fileStream = System.IO.File.Create(fullPath);
                    files[0].CopyTo(fileStream);
                    MenuItem.Image = @"\images\menuitems\" + fileName + extension;
                }
                _unitOfwork.MenuItem.Add(MenuItem);
            }
            else
            {

                var objFromDb = _unitOfwork.MenuItem.Get(m => m.Id == MenuItem.Id, true);

                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\menuitems\");
                    var extension = Path.GetExtension(files[0].FileName);
                    var imagePath = Path.Combine(webRootPath, objFromDb.Image.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    var fullPath = uploads + fileName + extension;
                    using var fileStream = System.IO.File.Create(fullPath);
                    files[0].CopyTo(fileStream);
                    MenuItem.Image = @"\images\menuitems\" + fileName + extension;
                }
                else
                {
                    MenuItem.Image = objFromDb.Image;
                }
            }
            _unitOfwork.MenuItem.Update(MenuItem);
            _unitOfwork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
