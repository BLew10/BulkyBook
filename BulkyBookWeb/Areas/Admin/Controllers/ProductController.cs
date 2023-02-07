using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
	public class ProductController : Controller
	{
		//this is the local implementation that we will use in our class
		private readonly IUnitOfWork _unitOfWork;
		//gives us access to the wwwroot folder
		private readonly IWebHostEnvironment _hostEnvironment;
		//we need to implement this unitOfWork.
		// what gets passed in here is the implementation of the unitOfWork that will be global in app
		public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_hostEnvironment = hostEnvironment;
		}
		public IActionResult Index()
		{

			return View();
		}



		//GET - EDIT
		public IActionResult Upsert(int? id)
		{
			ProductVM productVM = new()
			{
				Product = new(),
				CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				}),
				CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				}),
			};

			if (id == null || id == 0)
			{
				//create product
				//ViewBag.CategoryList = CategoryList;
				//ViewData["CoverTypeList"] = CoverTypeList;
				return View(productVM);
			}
			else
			{
				productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
				return View(productVM);

				//update product
			}


		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(ProductVM newOrEditedProduct, IFormFile? file)
		{
			if (ModelState.IsValid)
			{
				string webRootPath = _hostEnvironment.WebRootPath;
				if (file != null)
				{
					string fileName = Guid.NewGuid().ToString();
					var uploads = Path.Combine(webRootPath, @"images\products");
					var extension = Path.GetExtension(file.FileName);

					if (newOrEditedProduct.Product.ImageUrl != null)
					{
						//this is an edit and we need to remove old image
						var oldImagePath = Path.Combine(webRootPath, newOrEditedProduct.Product.ImageUrl.TrimStart('\\'));
						if (System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}

					using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
					{
						file.CopyTo(fileStreams);
					}

					newOrEditedProduct.Product.ImageUrl = @"\images\products\" + fileName + extension;
				}
				
				if(newOrEditedProduct.Product.Id == 0)
				{
					_unitOfWork.Product.Add(newOrEditedProduct.Product);
				}
				else
				{
					_unitOfWork.Product.Update(newOrEditedProduct.Product);
				}

				_unitOfWork.Save();
				TempData["success"] = "Product Updated Successfully";
				//looks for Index within same controller
				return RedirectToAction("Index");
				// if in another controller
				// return RedirectToAction("Index","OtherController");
			}

			return View(newOrEditedProduct);
		}

		#region API CALLS

		public IActionResult GetAll()
		{
			var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
			return Json(new { data = productList });
		}

		[HttpDelete]
		public IActionResult Delete(int? id)
		{

			//Find uses the priamry key, so we need to pass in the id. I have used FirstOrDefault previously
			Product RemovedProduct = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
			if (RemovedProduct == null)
			{
				return Json(new { success = false, message = "Error While Deleting"});
			}

			string webRootPath = _hostEnvironment.WebRootPath;
			var oldImagePath = Path.Combine(webRootPath, RemovedProduct.ImageUrl.TrimStart('\\'));
			if (System.IO.File.Exists(oldImagePath))
			{
				System.IO.File.Delete(oldImagePath);
			}
			_unitOfWork.Product.Remove(RemovedProduct);
			_unitOfWork.Save();
			return Json(new { success = true, message = " Product Deleted Successfully" });
		}
	}

	#endregion
}

