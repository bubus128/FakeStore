using FakeStore.Business.ProductService;
using FakeStore.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FakeStore.Presentation.UnitTests.HomeControllerTests;

[TestFixture]
public class IndexTests
{
	private Mock<IProductService> _mockProductService;
	private HomeController _controller;

	[SetUp]
	public void SetUp()
	{
		_mockProductService = new Mock<IProductService>();
		_controller = new HomeController(_mockProductService.Object);
	}

	[TearDown]
	public void Dispose()
		=> _controller.Dispose();


	[Test]
	public async Task Index_ShouldReturnView()
	{
		// Arrange
		var categories = new List<string> { "electronics", "jewelery" };
		_mockProductService.Setup(service => service.GetCategoriesAsync()).ReturnsAsync(categories);

		// Act
		var result = await _controller.Index();

		// Assert
		var viewResult = result as ViewResult;
		Assert.IsNotNull(viewResult);
	}

	[Test]
	public async Task Index_ShouldReturnViewWithCategories()
	{
		// Arrange
		var categories = new List<string> { "electronics", "jewelery" };
		_mockProductService.Setup(service => service.GetCategoriesAsync()).ReturnsAsync(categories);

		// Act
		var result = await _controller.Index();

		// Assert
		var viewResult = result as ViewResult;
		Assert.That(viewResult.Model, Is.EqualTo(categories));
	}
}