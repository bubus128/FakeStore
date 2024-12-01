using FakeStore.ApiClient.Models;
using FakeStore.Business.CartService;
using FakeStore.Business.ProductService;
using FakeStore.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FakeStore.Presentation.UnitTests.HomeControllerTests;

[TestFixture]
public class CategoryTests
{
	private Mock<IProductService> _mockProductService;
	private Mock<ICartService> _mockCartService;
	private Mock<ILogger<HomeController>> _mockLogger;
	private HomeController _controller;
	private List<Product> products;

	[SetUp]
	public void SetUp()
	{
		_mockProductService = new();
		_mockCartService = new();
		_mockLogger = new();
		_controller = new(_mockProductService.Object, _mockCartService.Object, _mockLogger.Object);
		products = new()
		{
			new()
			{
				Id = 1,
				Title = "Product 1",
				Description = null,
				Category = null,
				Image = null
			},
			new()
			{
				Id = 2,
				Title = "Product 2",
				Description = null,
				Category = null,
				Image = null
			}
		};
	}

	[TearDown]
	public void Dispose()
		=> _controller.Dispose();


	[Test]
	public async Task Category_ShouldReturnView()
	{
		// Arrange
		var category = "electronics";
		var pageSize = 10;
		int pageNumer = 1;
		_mockProductService
			.Setup(service => service.GetProductsByCategoryAsync(category, pageNumer, pageSize))
			.ReturnsAsync((products, 2));

		// Act
		var result = await _controller.Category(category);

		// Assert
		var viewResult = result as ViewResult;
		Assert.IsNotNull(viewResult);
	}

	[Test]
	public async Task Category_ShouldReturnViewWithProducts()
	{
		// Arrange
		var category = "electronics";
		var pageSize = 10;
		int pageNumer = 1;
		_mockProductService
			.Setup(service => service.GetProductsByCategoryAsync(category, pageNumer, pageSize))
			.ReturnsAsync((products, 2));

		// Act
		var result = await _controller.Category(category);

		// Assert
		var viewResult = result as ViewResult;
		Assert.That(viewResult.Model, Is.EqualTo(products));
	}

	[Test]
	public async Task Category_ShouldReturnViewWithCorrectPageSize()
	{
		// Arrange
		var category = "electronics";
		var pageSize = 10;
		int pageNumer = 1;
		_mockProductService
			.Setup(service => service.GetProductsByCategoryAsync(category, pageNumer, pageSize))
			.ReturnsAsync((products, 2));

		// Act
		var result = await _controller.Category(category);

		// Assert
		var viewResult = result as ViewResult;
		Assert.AreEqual(pageSize, _controller.ViewBag.PageSize);
	}

	[Test]
	public async Task Category_ShouldReturnViewWithCorrectPageNumber()
	{
		// Arrange
		var category = "electronics";
		var pageSize = 10;
		int pageNumer = 1;
		_mockProductService
			.Setup(service => service.GetProductsByCategoryAsync(category, pageNumer, pageSize))
			.ReturnsAsync((products, 2));

		// Act
		var result = await _controller.Category(category);

		// Assert
		var viewResult = result as ViewResult;
		Assert.AreEqual(pageNumer, _controller.ViewBag.CurrentPage);
	}

	[Test]
	public async Task Category_ShouldReturnViewWithCorrectTotalPages()
	{
		// Arrange
		var category = "electronics";
		var pageSize = 1;
		int pageNumer = 1;
		int totalPages = 2;
		_mockProductService
			.Setup(service => service.GetProductsByCategoryAsync(category, pageNumer, pageSize))
			.ReturnsAsync((products, 2));

		// Act
		var result = await _controller.Category(category, pageNumer, pageSize);

		// Assert
		Assert.AreEqual(totalPages, _controller.ViewBag.TotalPages);
	}

	[Test]
	public async Task Category_ShouldReturnViewWithCorrectCategory()
	{
		// Arrange
		var category = "electronics";
		var pageSize = 10;
		int pageNumer = 1;
		_mockProductService
			.Setup(service => service.GetProductsByCategoryAsync(category, pageNumer, pageSize))
			.ReturnsAsync((products, 2));

		// Act
		var result = await _controller.Category(category);

		// Assert
		var viewResult = result as ViewResult;
		Assert.AreEqual(category, _controller.ViewBag.CategoryName);
	}
}