using FakeStore.Business.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace FakeStore.Presentation.Controllers;

public class HomeController : Controller
{
	private readonly IProductService _productService;

	public HomeController(IProductService productService)
	{
		_productService = productService;
	}

	public async Task<IActionResult> Index()
	{
		var categories = await _productService.GetCategoriesAsync();
		return View(categories);
	}

	public async Task<IActionResult> Category(string name, int page = 1, int pageSize = 10)
	{
		var (products, totalProducts) = await _productService.GetProductsByCategoryAsync(name, page, pageSize);

		ViewBag.CurrentPage = page;
		ViewBag.PageSize = pageSize;
		ViewBag.TotalPages = (int)System.Math.Ceiling((double)totalProducts / pageSize);
		ViewBag.CategoryName = name;

		return View(products);
	}
}

