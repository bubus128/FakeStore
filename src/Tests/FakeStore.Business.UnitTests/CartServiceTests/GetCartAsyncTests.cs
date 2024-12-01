using FakeStore.ApiClient.Models;
using FakeStore.ApiCLient.FakeStoreApiClient;
using Microsoft.Extensions.Logging;
using Moq;

namespace FakeStore.Business.UnitTests.CartServiceTests;

[TestFixture]
public class GetCartAsyncTests
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
	public async Task GetCartAsync_ShouldReturnCart()
	{
		// Arrange
		var userId = 1;
		var expectedCart = new Cart
		{
			Id = 10,
			Products = null
		};
		_mockApiClient.Setup(api => api.GetCartAsyncByUserId(userId)).ReturnsAsync(expectedCart);

		// Act
		var result = await _cartService.GetCartAsync(userId);

		// Assert
		Assert.That(result, Is.EqualTo(expectedCart));
	}

	[Test]
	public void GetCartAsync_ShouldLogError_WhenApiClientThrowsException()
	{
		// Arrange
		var userId = 1;
		_mockApiClient.Setup(api => api.GetCartAsyncByUserId(userId)).ThrowsAsync(new Exception("API error"));

		// Act & Assert
		var ex = Assert.ThrowsAsync<Exception>(async () => await _cartService.GetCartAsync(userId));
		Assert.That(ex.Message, Is.EqualTo("API error"));
	}
}