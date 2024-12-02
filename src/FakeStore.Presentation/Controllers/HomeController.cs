using FakeStore.Business.CartService;
using FakeStore.Business.ProductService;
using FakeStore.Models;
using FakeStore.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace FakeStore.Presentation.Controllers;

public class HomeController(IProductService productService, ICartService cartService, ILogger<HomeController> logger) : Controller
{
    public async Task<IActionResult> Index()
    {
        try
        {
            var categories = await productService.GetCategoriesAsync().ConfigureAwait(false);
            return View(categories);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while loading home page");
            return View("Error", new ErrorViewModel { Message = "The category failed to load." });
        }
    }

    public async Task<IActionResult> Category(string name, int page = 1, int pageSize = 10)
    {
        try
        {
            var (products, totalProducts) = await productService.GetProductsByCategoryAsync(name, page, pageSize).ConfigureAwait(false);

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CategoryName = name;

            return View(products);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while loading categories {name}");
            return View("Error", new ErrorViewModel { Message = "Products from the selected category could not be loaded." });
        }
    }

    public async Task<IActionResult> Product(int id)
    {
        try
        {
            var product = await productService.GetProductAsync(id).ConfigureAwait(false);
            return View(product);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while loading product id {id}");
            return View("Error", new ErrorViewModel { Message = "Product could not be loaded." });
        }

    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId)
    {
        // Force to use existing user
        int userId = 1;
        try
        {
            await cartService.AddToCartAsync(userId, productId).ConfigureAwait(false);
            return RedirectToAction("Cart");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while adding to cart");
            return View("Error", new ErrorViewModel { Message = "Product could not be added to cart." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        // Force to use existing user
        int userId = 1;
        try
        {
            await cartService.RemoveFromCartAsync(userId, productId).ConfigureAwait(false);
            return RedirectToAction("Cart");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while removing product {productId} from users {userId} cart");
            return View("Error", new ErrorViewModel { Message = "Product could not be removed drom cart." });
        }

    }

    public async Task<IActionResult> Cart()
    {
        // Force to use existing user
        int userId = 1;

        try
        {
            var cart = await cartService.GetCartAsync(userId).ConfigureAwait(false);
            return View(await Task.WhenAll(cart.Products.Select(async product => new CartViewModel()
            {
                Quantity = product.Quantity,
                product = await productService.GetProductAsync(product.ProductId).ConfigureAwait(false)
            }).ToList()).ConfigureAwait(false));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while loading the cart");
            return View("Error", new ErrorViewModel { Message = "Cart could not be loaded." });
        }
    }
}

