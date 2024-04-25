using Lookeach.Core.Repositories;

namespace Lookeach.Core.UnitOfWorks
{
    public interface IUnitOfWork 
    {
        IAccessTokenRepository AccessTokenRepository { get; }
        IBrandRepository BrandRepository { get; }
        ICartRepository CartRepository { get; }
        ICartItemRepository CartItemRepository { get; }
        ICategoryReposiitory CategoryRepository { get; }
        IChatRepository ChatRepository { get; }
        IChatUserRepository ChatUserRepository { get; }
        IColorRepository ColorRepository { get; }
        IGenderTypeRepository GenderTypeRepository { get; }
        IImageRepository ImageRepository { get; }
        IMessageRepository MessageRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }
        IPaymentMethodRepository PaymentMethodRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductSourcingRepository ProductSourcingRepository { get; }
        IProductVoucherRepository ProductVoucherRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IRoleRepository RoleRepository { get; }
        IShopVoucherRepository ShopVoucherRepository { get; }
        ISizeRepository SizeRepository { get; }
        IUserRepository UserRepository { get; }
        IUserClaimRepository UserClaimRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IVoucherRepository VoucherRepository { get; }
        Task<int> SaveChangesAsync();

    }
}
