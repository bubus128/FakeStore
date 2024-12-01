using FakeStore.ApiClient.Models;
using System.Text.Json;

namespace FakeStore.ApiCLient.FakeStoreApiClient;

public class FakeStoreApiClient(HttpClient httpClient) : IFakeStoreApiClient
{
	public async Task<List<string>> GetCategoriesAsync()
	{
		var response = await httpClient.GetStringAsync("https://fakestoreapi.com/products/categories");
		return JsonSerializer.Deserialize<List<string>>(response, new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		});
	}

	public async Task<List<Product>> GetProductsByCategoryAsync(string category)
	{
		var response = await httpClient.GetStringAsync($"https://fakestoreapi.com/products/category/{category}");
		return JsonSerializer.Deserialize<List<Product>>(response, new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		});
	}
}

