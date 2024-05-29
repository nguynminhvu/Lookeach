using Lookeach.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Interfaces
{
    public interface ICleanContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<AccessToken> AccessTokens { get; set; }
        DbSet<Brand> Brands { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Chat> Chats { get; set; }
        DbSet<Color> Colors { get; set; }
        DbSet<GenderType> GenderTypes { get; set; }
        DbSet<Image> Images { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderItem> OrderItems { get; set; }
        DbSet<PaymentMethod> PaymentMethods { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductSourcing> ProductSourcings { get; set; }
        DbSet<Size> Sizes { get; set; }
        DbSet<Voucher> Vouchers { get; set; }
        DbSet<ProductImage> ProductImages { get; set; }
        DbSet<ProductSourcingImage> ProductSourcingImages { get; set; }
        DbSet<CategoryImage> CategoryImages { get; set; }
        DbSet<BrandImage> BrandImages { get; set; }
        DbSet<ChatImage> ChatImages { get; set; }
        DbSet<VoucherSourcing> VoucherSourcings { get; set; }
        DbSet<Resource> Resources { get; set; }
        DbSet<Lookeach.Core.Entities.Action> Actions { get; set; }
        DbSet<Permission> Permissions { get; set; }
        DbSet<TransactionMaster> TransactionMasters { get; set; }
        DbSet<VNPayTransaction> VNPayTransactions { get; set; }
        DbSet<VNPayTransactionRefund> VNPayTransactionRefunds { get; set; }
        DbSet<PayPalOrder> PayPalOrders { get; set; }
        DbSet<PayPalAmount> PayPalAmounts { get; set; }
        DbSet<PayPalItem> PayPalItems { get; set; }
        DbSet<ChatUser> ChatUsers { get; set; }
        DbSet<ChatMessage> ChatMessages { get; set; }
        DbSet<ParticipantChat> ParticipantChats { get; set; }
        DbSet<SuperAdmin> SuperAdmins { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
