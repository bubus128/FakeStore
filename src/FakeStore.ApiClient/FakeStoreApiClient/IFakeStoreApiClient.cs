using FakeStore.ApiClient.Models;

namespace FakeStore.ApiCLient.FakeStoreApiClient;

public interface IFakeStoreApiClient
{
	public Task<List<string>> GetCategoriesAsync();
	public Task<List<Product>> GetProductsByCategoryAsync(string category);

}