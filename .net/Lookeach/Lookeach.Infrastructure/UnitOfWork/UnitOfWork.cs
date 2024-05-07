using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories;

namespace Lookeach.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LookeachContext _context;

        public UnitOfWork(LookeachContext lookeachContext)
        {
            _context = lookeachContext;
            AccessTokenRepository = new AccessTokenRepository(_context);
            BrandRepository = new BrandRepository(_context);
            CategoryRepository = new CategoryRepository(_context);
            ChatRepository = new ChatRepository(_context);
            ChatUserRepository = new ChatUserRepository(_context);
            ColorRepository = new ColorRepository(_context);
            GenderTypeRepository = new GenderTypeRepository(_context);
            ImageRepository = new ImageRepository(_context);
            MessageRepository = new MessageRepository(_context);
            OrderRepository = new OrderRepository(_context);
            OrderItemRepository = new OrderItemRepository(_context);
            PaymentMethodRepository = new PaymentMethodRepository(_context);
            ProductRepository = new ProductRepository(_context);
            ProductSourcingRepository = new ProductSourcingRepository(_context);
            RoleRepository = new RoleRepository(_context);
            SizeRepository = new SizeRepository(_context);
            UserRepository = new UserRepository(_context);
            VoucherRepository = new VoucherRepository(_context);
            BrandImageRepository = new BrandImageRepository(_context);
            CategoryImageRepository = new CategoryImageRepository(_context);
            ProductImageRepository = new ProductImageRepository(_context);
            ProductSourcingImageRepository = new ProductSourcingImageRepository(_context);
            VoucherSourcingRepository = new VoucherSourcingRepository(_context);
        }

        public IAccessTokenRepository AccessTokenRepository { get; private set; }

        public IBrandRepository BrandRepository { get; private set; }

        public ICategoryReposiitory CategoryRepository { get; private set; }

        public IChatRepository ChatRepository { get; private set; }

        public IChatUserRepository ChatUserRepository { get; private set; }

        public IColorRepository ColorRepository { get; private set; }

        public IGenderTypeRepository GenderTypeRepository { get; private set; }

        public IImageRepository ImageRepository { get; private set; }

        public IMessageRepository MessageRepository { get; private set; }

        public IOrderRepository OrderRepository { get; private set; }

        public IOrderItemRepository OrderItemRepository { get; private set; }

        public IPaymentMethodRepository PaymentMethodRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }

        public IProductSourcingRepository ProductSourcingRepository { get; private set; }

        public IRoleRepository RoleRepository { get; private set; }

        public ISizeRepository SizeRepository { get; private set; }

        public IUserRepository UserRepository { get; private set; }

        public IVoucherRepository VoucherRepository { get; private set; }

        public IBrandImageRepository BrandImageRepository { get; private set; }

        public ICategoryImageRepository CategoryImageRepository { get; private set; }

        public IProductImageRepository ProductImageRepository { get; private set; }

        public IProductSourcingImageRepository ProductSourcingImageRepository { get; private set; }

        public IVoucherSourcingRepository VoucherSourcingRepository { get; private set; }

        public async Task<int> SaveChangesAsync()
          => await _context.SaveChangesAsync();
    }
}
