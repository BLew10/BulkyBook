using BulkyBook.DataAccess;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.Models;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        //this is the local implementation that we will use in our class
        private readonly IUnitOfWork _unitOfWork;

        //we need to implement this unitOfWork.
        // what gets passed in here is the implementation of the unitOfWork that will be global in app
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            //our "model" will just be on the cshtml page. it will be of type "Category". This will be the model we will need in order to create another Category 

            return View();

        }


        [HttpPost]
        //Prevents cross site forgery attack. Within any forms, it injects an key and the key will be validated here
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category newCategory)
        {
            // Custom Error Message
            if (newCategory.Name == newCategory.DisplayOrder.ToString())
            {
                ModelState.AddModelError("DisplayOrder", "Display Order and Name cannot be the same");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(newCategory);
                _unitOfWork.Save();
                TempData["success"] = "Category Created Successfully";

                //looks for Index within same controller
                return RedirectToAction("Index");
                // if in another controller
                // return RedirectToAction("Index","OtherController");
            }

            return View(newCategory);

        }

        //GET - EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Find uses the priamry key, so we need to pass in the id. I have used FirstOrDefault previously
            Category catFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            // Category catFirst = _unitOfWork.Category.Category.FirstOrDefault(c => c.Id == id);
            // Category catSingle = _unitOfWork.Category.Category.SingleOrDefault(c => c.Id == id);

            if (catFromDb == null)
            {
                return NotFound();
            }

            return View(catFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category editedCategory)
        {
            if (editedCategory.Name == editedCategory.DisplayOrder.ToString())
            {
                ModelState.AddModelError("DisplayOrder", "Display Order and Name cannot be the same");
            }
            if (ModelState.IsValid)
            {

                _unitOfWork.Category.Update(editedCategory);
                _unitOfWork.Save();
                TempData["success"] = "Category Updated Successfully";
                //looks for Index within same controller
                return RedirectToAction("Index");
                // if in another controller
                // return RedirectToAction("Index","OtherController");
            }

            return View(editedCategory);
        }

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Find uses the priamry key, so we need to pass in the id. I have used FirstOrDefault previously
            Category catFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            // Category catFirst = _unitOfWork.Category.Category.FirstOrDefault(c => c.Id == id);
            // Category catSingle = _unitOfWork.Category.Category.SingleOrDefault(c => c.Id == id);

            if (catFromDb == null)
            {
                return NotFound();
            }

            return View(catFromDb);
        }


        [HttpPost]
        public IActionResult DeletePost(int? id)
        {

            //Find uses the priamry key, so we need to pass in the id. I have used FirstOrDefault previously
            Category RemovedCategory = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (RemovedCategory == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(RemovedCategory);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
