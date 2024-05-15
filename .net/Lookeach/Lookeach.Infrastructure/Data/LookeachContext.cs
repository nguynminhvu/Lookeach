using Lookeach.Application.Interfaces;
using Lookeach.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Lookeach.Infrastructure.Data
{
    public class LookeachContext : DbContext, ICleanContext
    {
        public LookeachContext()
        {

        }
        public LookeachContext(DbContextOptions<LookeachContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=Lookeach;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");


        private string GetConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, true).Build();
            return configuration["ConnectionStrings:LookeachDb"] + "";
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<GenderType> GenderTypes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSourcing> ProductSourcings { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductSourcingImage> ProductSourcingImages { get; set; }
        public DbSet<CategoryImage> CategoryImages { get; set; }
        public DbSet<BrandImage> BrandImages { get; set; }
        public DbSet<ChatImage> ChatImages { get; set; }
        public DbSet<VoucherSourcing> VoucherSourcings { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Lookeach.Core.Entities.Action> Actions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<TransactionMaster> TransactionMasters { get; set; }
        public DbSet<VNPayTransaction> VNPayTransactions { get; set; }
        public DbSet<VNPayTransactionRefund> VNPayTransactionRefunds { get; set; }
        public DbSet<PayPalOrder> PayPalOrders { get; set; }
        public DbSet<PayPalAmount> PayPalAmounts { get; set; }
        public DbSet<PayPalItem> PayPalItems { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
