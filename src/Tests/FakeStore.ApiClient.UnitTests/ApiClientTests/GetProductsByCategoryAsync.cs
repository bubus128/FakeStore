using FakeStore.ApiCLient.FakeStoreApiClient;
using Moq;

namespace FakeStore.ApiClient.UnitTests.ApiClientTests;

[TestFixture]
public class GetProductsByCategoryAsync
{
	private Mock<HttpMessageHandler> _mockHandler;
	private FakeStoreApiClient _apiClient;

	[SetUp]
	public void SetUp()
	{
		_mockHandler = new Mock<HttpMessageHandler>();
		var httpClient = new HttpClient(_mockHandler.Object);
		_apiClient = new FakeStoreApiClient(httpClient);
	}
}