using BulkyBook.DataAccess;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.Models;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class CoverTypeController : Controller
    {
        //this is the local implementation that we will use in our class
        private readonly IUnitOfWork _unitOfWork;

        //we need to implement this unitOfWork.
        // what gets passed in here is the implementation of the unitOfWork that will be global in app
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View(objCoverTypeList);
        }

        public IActionResult Create()
        {
            //our "model" will just be on the cshtml page. it will be of type "CoverType". This will be the model we will need in order to create another CoverType 

            return View();

        }


        [HttpPost]
        //Prevents cross site forgery attack. Within any forms, it injects an key and the key will be validated here
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType newCoverType)
        {
           
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(newCoverType);
                _unitOfWork.Save();
                TempData["success"] = "CoverType Created Successfully";

                //looks for Index within same controller
                return RedirectToAction("Index");
                // if in another controller
                // return RedirectToAction("Index","OtherController");
            }

            return View(newCoverType);

        }

        //GET - EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Find uses the priamry key, so we need to pass in the id. I have used FirstOrDefault previously
            CoverType coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            // CoverType catFirst = _unitOfWork.CoverType.CoverType.FirstOrDefault(c => c.Id == id);
            // CoverType catSingle = _unitOfWork.CoverType.CoverType.SingleOrDefault(c => c.Id == id);

            if (coverTypeFromDb == null)
            {
                return NotFound();
            }

            return View(coverTypeFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType editedCoverType)
        {

            if (ModelState.IsValid)
            {

                _unitOfWork.CoverType.Update(editedCoverType);
                _unitOfWork.Save();
                TempData["success"] = "CoverType Updated Successfully";
                //looks for Index within same controller
                return RedirectToAction("Index");
                // if in another controller
                // return RedirectToAction("Index","OtherController");
            }

            return View(editedCoverType);
        }

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Find uses the priamry key, so we need to pass in the id. I have used FirstOrDefault previously
            CoverType coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            // CoverType catFirst = _unitOfWork.CoverType.CoverType.FirstOrDefault(c => c.Id == id);
            // CoverType catSingle = _unitOfWork.CoverType.CoverType.SingleOrDefault(c => c.Id == id);

            if (coverTypeFromDb == null)
            {
                return NotFound();
            }

            return View(coverTypeFromDb);
        }


        [HttpPost]
        public IActionResult DeletePost(int? id)
        {

            //Find uses the priamry key, so we need to pass in the id. I have used FirstOrDefault previously
            CoverType RemovedCoverType = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (RemovedCoverType == null)
            {
                return NotFound();
            }

            _unitOfWork.CoverType.Remove(RemovedCoverType);
            _unitOfWork.Save();
            TempData["success"] = "CoverType Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
