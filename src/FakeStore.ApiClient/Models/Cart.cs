namespace FakeStore.ApiClient.Models;

public class Cart
{
    public int Id { get; set; }
    public required List<CartProduct> Products { get; set; }
}