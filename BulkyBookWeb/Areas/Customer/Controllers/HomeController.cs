using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace BulkyBookWeb.Controllers;
[Area("Customer")]
public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private readonly IUnitOfWork _unitOfWork;

	public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
	{
		_logger = logger;
		_unitOfWork = unitOfWork;
	}

	public IActionResult Index()
	{
		IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");

		return View(productList);
	}

	public IActionResult Details(int id)
	{


		ShoppingCart cartObj = new()
		{
			Count = 1,
			ProductId = id,
			Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id, includeProperties: "Category,CoverType"),
		};
		return View(cartObj);
	}




public IActionResult Privacy()
{
	return View();
}

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public IActionResult Error()
{
	return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}

}
