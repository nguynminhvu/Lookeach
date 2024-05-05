namespace Lookeach.Core.ViewModels.Carts
{
    public class CartUpdateModel
    {
        public decimal Amount { get; set; }
        public ICollection<CartItemUpdateModel> CartItems { get; set; }
    }
}
