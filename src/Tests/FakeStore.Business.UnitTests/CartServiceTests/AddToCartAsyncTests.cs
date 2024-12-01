using FakeStore.ApiClient.Models;
using FakeStore.ApiCLient.FakeStoreApiClient;
using Microsoft.Extensions.Logging;
using Moq;

namespace FakeStore.Business.UnitTests.CartServiceTests;

[TestFixture]
public class AddToCartAsyncTests
{
	private Mock<IFakeStoreApiClient> _mockApiClient;
	private CartService.CartService _cartService;
	private Mock<ILogger<CartService.CartService>> _mockLogger;

	[SetUp]
	public void SetUp()
	{
		_mockApiClient = new Mock<IFakeStoreApiClient>();
		_mockLogger = new Mock<ILogger<CartService.CartService>>();
		_cartService = new CartService.CartService(_mockApiClient.Object, _mockLogger.Object);
	}

	[Test]
	public async Task AddToCartAsync_ShouldCallApiClientAddToCartAsync()
	{
		// Arrange
		var userId = 1;
		var productId = 2;
		var cart = new Cart
		{
			Id = 5,
			Products = null
		};
		_mockApiClient.Setup(api => api.GetCartAsyncByUserId(userId)).ReturnsAsync(cart);

		// Act
		await _cartService.AddToCartAsync(userId, productId);

		// Assert
		_mockApiClient.Verify(api => api.AddToCartAsync(cart.Id, productId), Times.Once);
	}

	[Test]
	public void AddToCartAsync_ShouldLogError_WhenApiClientThrowsException()
	{
		// Arrange
		var userId = 1;
		var productId = 2;
		_mockApiClient.Setup(api => api.GetCartAsyncByUserId(userId)).ThrowsAsync(new Exception("API error"));

		// Act & Assert
		var ex = Assert.ThrowsAsync<Exception>(async () => await _cartService.AddToCartAsync(userId, productId));
		Assert.That(ex.Message, Is.EqualTo("API error"));
	}
}