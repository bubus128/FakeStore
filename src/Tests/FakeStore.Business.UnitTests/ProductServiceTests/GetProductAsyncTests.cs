using FakeStore.ApiClient.Models;
using FakeStore.ApiCLient.FakeStoreApiClient;
using Microsoft.Extensions.Logging;
using Moq;

namespace FakeStore.Business.UnitTests.ProductServiceTests;

[TestFixture]
public class GetProductAsyncTests
{
	private Mock<IFakeStoreApiClient> _mockApiClient;
	private ProductService.ProductService _productService;
	private Mock<ILogger<ProductService.ProductService>> _mockLogger;

	[SetUp]
	public void SetUp()
	{
		_mockApiClient = new Mock<IFakeStoreApiClient>();
		_mockLogger = new Mock<ILogger<ProductService.ProductService>>();
		_productService = new ProductService.ProductService(_mockApiClient.Object, _mockLogger.Object);
	}

	[Test]
	public async Task GetProductAsync_ShouldReturnProduct()
	{
		// Arrange
		var productId = 1;
		var expectedProduct = new Product
		{
			Id = productId,
			Title = "Product 1",
			Description = null,
			Category = null,
			Image = null
		};
		_mockApiClient.Setup(api => api.GetProductById(productId)).ReturnsAsync(expectedProduct);

		// Act
		var result = await _productService.GetProductAsync(productId);

		// Assert
		Assert.That(result.Id, Is.EqualTo(productId));
	}

	[Test]
	public void GetProductAsync_ShouldLogError_WhenApiClientThrowsException()
	{
		// Arrange
		var productId = 1;
		_mockApiClient.Setup(api => api.GetProductById(productId)).ThrowsAsync(new Exception("API error"));

		// Act & Assert
		var ex = Assert.ThrowsAsync<Exception>(async () => await _productService.GetProductAsync(productId));
		Assert.That(ex.Message, Is.EqualTo("API error"));
	}
}