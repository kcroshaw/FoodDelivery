using Microsoft.AspNetCore.Http;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.wwwroot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodTypeController : Controller
    {
        private readonly IUnitOfWork _unitofWork;

        public FoodTypeController(IUnitOfWork unitOfWork) => _unitofWork = unitOfWork;

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitofWork.FoodType.List() });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitofWork.FoodType.Get(c => c.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitofWork.FoodType.Delete(objFromDb);
            _unitofWork.Commit();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}
