using FakeStore.ApiClient.Models;

namespace FakeStore.Presentation.Models;

public class CartViewModel
{
	public required Product product { get; set; }
	public int Quantity { get; set; }
}