using Lookeach.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Lookeach.Infrastructure.Data
{
    public class LookeachContext : DbContext
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
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryVoucher> CategoryVouchers { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<GenderType> GenderTypes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSourcing> ProductSourcings { get; set; }
        public DbSet<ProductVoucher> ProductVouchers { get; set; }
        public DbSet<ShopVoucher> ShopVouchers { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductSourcingImage> ProductSourcingImages { get; set; }
        public DbSet<CategoryImage> CategoryImages { get; set; }
        public DbSet<BrandImage> BrandImages { get; set; }
        public DbSet<ChatImage> ChatImages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasOne(r => r.Role)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.RoleId)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .HasMany(x => x.Users)
                .WithOne(x => x.Role)
                .IsRequired();

            //modelBuilder.Entity<Product>(x =>
            //{
            //    x.HasOne(x => x.Brand).WithMany().HasForeignKey(x => x.BrandId);
            //    x.HasMany(x => x.Sizes).WithMany(x => x.Products).UsingEntity("ProductSize",
            //        s => s.HasOne(typeof(Size)).WithMany().HasForeignKey("SizeId").HasPrincipalKey(nameof(Size.Id)),
            //        p => p.HasOne(typeof(Product)).WithMany().HasForeignKey("ProductId").HasPrincipalKey(nameof(Product.Id)),
            //        k => k.HasKey("SizeId", "ProductId")
            //        );
            //    x.HasMany(x => x.Colors).WithMany(x => x.Products);
            //    x.HasMany(x => x.Categories).WithMany(x => x.Products);
            //});

        }
    }
}


//public string Name { get; set; }
//public int Quantity { get; set; }
//public decimal Price { get; set; }
//public string Description { get; set; }
//public ICollection<Image>? Images { get; set; }
//public ICollection<Size>? Sizes { get; set; }
//public ICollection<Color>? Colors { get; set; }
//public Guid BrandId { get; set; }
//public Brand Brand { get; set; }