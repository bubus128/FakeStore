using FakeStore.ApiClient.Models;
using FakeStore.ApiCLient.FakeStoreApiClient;
using Microsoft.Extensions.Logging;
using Moq;

namespace FakeStore.Business.UnitTests.ProductServiceTests;

[TestFixture]
public class GetCategoriesAsyncTests
{
	private Mock<IFakeStoreApiClient> _mockApiClient;
	private ProductService.ProductService _productService;
	private Mock<ILogger<ProductService.ProductService>> _mockLogger;


	[SetUp]
	public void SetUp()
	{
		_mockLogger = new();
		_mockApiClient = new();
		_productService = new ProductService.ProductService(_mockApiClient.Object, _mockLogger.Object);
	}

	[Test]
	public async Task GetCategoriesAsync_ShouldReturnCategories()
	{
		// Arrange
		var categories = new List<string> { "electronics", "jewelery", "men's clothing", "women's clothing" };
		_mockApiClient.Setup(api => api.GetCategoriesAsync()).ReturnsAsync(categories);

		// Act
		var result = await _productService.GetCategoriesAsync();

		// Assert
		Assert.That(result, Is.EqualTo(categories));
	}

	[Test]
	public async Task GetCategoriesAsync_ShouldCall_ApiCLientGetCategoriesAsync()
	{
		// Arrange
		_mockApiClient.Setup(api => api.GetCategoriesAsync());

		// Act
		var result = await _productService.GetCategoriesAsync();

		// Assert
		_mockApiClient.Verify(api => api.GetCategoriesAsync(), Times.Once);
	}

	[Test]
	public async Task GetProductsByCategoryAsync_ShouldReturnPaginatedProducts()
	{
		// Arrange
		var category = "electronics";
		var products = new List<Product>
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
			},
			new()
			{
				Id = 3,
				Title = "Product 3",
				Description = null,
				Category = null,
				Image = null
			}
		};

		_mockApiClient.Setup(api => api.GetProductsByCategoryAsync(category)).ReturnsAsync(products);

		// Act
		var (paginatedProducts, totalProducts) = await _productService.GetProductsByCategoryAsync(category, 1, 2);

		// Assert
		Assert.That(paginatedProducts.Count, Is.EqualTo(2));
		Assert.That(totalProducts, Is.EqualTo(3));
		Assert.That(paginatedProducts[0].Title, Is.EqualTo("Product 1"));
		_mockApiClient.Verify(api => api.GetProductsByCategoryAsync(category), Times.Once);
	}
}