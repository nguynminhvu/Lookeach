using Lookeach.Core.Repositories;

namespace Lookeach.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IAccessTokenRepository AccessTokenRepository { get; }
        IBrandRepository BrandRepository { get; }
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
        IRoleRepository RoleRepository { get; }
        ISizeRepository SizeRepository { get; }
        IUserRepository UserRepository { get; }
        IVoucherRepository VoucherRepository { get; }
        IBrandImageRepository BrandImageRepository { get; }
        ICategoryImageRepository CategoryImageRepository { get; }
        IProductImageRepository ProductImageRepository { get; }
        IProductSourcingImageRepository ProductSourcingImageRepository { get; }
        Task<int> SaveChangesAsync();

    }
}
