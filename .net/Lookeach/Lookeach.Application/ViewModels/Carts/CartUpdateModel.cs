namespace Lookeach.Application.ViewModels.Carts
{
    public class CartUpdateModel
    {
        public decimal Amount { get; set; }
        public ICollection<CartItemUpdateModel> CartItems { get; set; }
    }
}
