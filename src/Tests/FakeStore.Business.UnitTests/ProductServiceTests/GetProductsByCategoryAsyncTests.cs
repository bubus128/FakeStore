using FakeStore.ApiClient.Models;
using FakeStore.ApiCLient.FakeStoreApiClient;
using Moq;

namespace FakeStore.Business.UnitTests.ProductServiceTests;

[TestFixture]
public class GetProductsByCategoryAsyncTests
{
	private Mock<IFakeStoreApiClient> _mockApiClient;
	private ProductService.ProductService _productService;
	private List<Product> products;

	[SetUp]
	public void SetUp()
	{
		_mockApiClient = new Mock<IFakeStoreApiClient>();
		_productService = new ProductService.ProductService(_mockApiClient.Object);
		products =
		[
			new Product
			{
				Id = 1,
				Title = "Product 1",
				Description = null,
				Category = null,
				Image = null
			},
			new Product
			{
				Id = 2,
				Title = "Product 2",
				Description = null,
				Category = null,
				Image = null
			},
			new Product
			{
				Id = 3,
				Title = "Product 3",
				Description = null,
				Category = null,
				Image = null
			}
		];
	}

	[Test]
	public async Task GetProductsByCategoryAsync_ShouldReturnPaginatedProducts_FullPage()
	{
		// Arrange
		var category = "electronics";
		var pageSize = 2;
		var pageNumber = 1;
		var expectedPaginatedProducts = new List<Product>
		{
			products[0],
			products[1]
		};

		_mockApiClient.Setup(api => api.GetProductsByCategoryAsync(category)).ReturnsAsync(products);

		// Act
		var (paginatedProducts, totalProducts) = await _productService.GetProductsByCategoryAsync(category, pageNumber, pageSize);

		// Assert
		Assert.That(paginatedProducts, Is.EqualTo(expectedPaginatedProducts));
	}

	[Test]
	public async Task GetProductsByCategoryAsync_ShouldReturnPaginatedProducts_NotFullPage()
	{
		// Arrange
		var category = "electronics";
		var pageSize = 2;
		var pageNumber = 2;
		var expectedPaginatedProducts = new List<Product>
		{
			products[2]
		};
		_mockApiClient.Setup(api => api.GetProductsByCategoryAsync(category)).ReturnsAsync(products);

		// Act
		var (paginatedProducts, totalProducts) = await _productService.GetProductsByCategoryAsync(category, pageNumber, pageSize);

		// Assert
		Assert.That(paginatedProducts, Is.EqualTo(expectedPaginatedProducts));
	}

	[Test]
	public async Task GetProductsByCategoryAsync_ShouldReturnTotalProductsCount()
	{
		// Arrange
		var category = "electronics";
		_mockApiClient.Setup(api => api.GetProductsByCategoryAsync(category)).ReturnsAsync(products);

		// Act
		var (paginatedProducts, totalProducts) = await _productService.GetProductsByCategoryAsync(category, 1, 2);

		// Assert
		Assert.That(totalProducts, Is.EqualTo(3));
	}

	[Test]
	public async Task GetProductsByCategoryAsync_ShouldCallGetProductsByCategoryAsync()
	{
		// Arrange
		var category = "electronics";
		_mockApiClient.Setup(api => api.GetProductsByCategoryAsync(category)).ReturnsAsync(products);

		// Act
		var (paginatedProducts, totalProducts) = await _productService.GetProductsByCategoryAsync(category, 1, 2);

		// Assert
		_mockApiClient.Verify(api => api.GetProductsByCategoryAsync(category), Times.Once);
	}
}

