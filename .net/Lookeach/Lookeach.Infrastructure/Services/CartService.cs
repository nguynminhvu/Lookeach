using Lookeach.Application.AutoMappers;
using Lookeach.Application.Services;
using Lookeach.Application.ViewModels.Carts;
using Lookeach.Application.ViewModels.Products;
using Lookeach.Application.ViewModels.Users;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Lookeach.Infrastructure.Services
{
    public class CartService : TokenService, ICartService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public CartService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer) : base(unitOfWork, configuration, httpContextAccessor, connectionMultiplexer, distributedCache)
        {
            _uow = unitOfWork;
            _configuration = configuration;
            _httpAccessor = httpContextAccessor;
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
            _productRepository = _uow.ProductRepository;
            _userRepository = _uow.UserRepository;
        }

        public async Task<CartViewModel> GetCart()
        {
            var claim = await GetClaimDTOByAccessToken();
            if (claim == null) throw new UnauthorizedAccessException("Login session expired");
            var cart = await _distributedCache.GetStringAsync(claim.Id + "");
            if (cart == null!)
            {
                CartViewModel cartViewModel = new CartViewModel
                {
                    Id = Guid.NewGuid(),
                    Amount = 0,
                    UserId = claim.Id,
                    CartItems = null!
                };
                await _distributedCache.SetStringAsync(claim.Id + "", JsonConvert.SerializeObject(cartViewModel));
                return cartViewModel;
            }
            return JsonConvert.DeserializeObject<CartViewModel>(cart)!;
        }

        public async Task<CartViewModel> UpdateCart(CartUpdateModel cartUpdateModel)
        {
            var claim = await GetClaimDTOByAccessToken();
            if (claim == null!) throw new UnauthorizedAccessException("Login session expired");
            var cartString = await _distributedCache.GetStringAsync(claim.Id + "");
            var user = await _userRepository.GetUserById(claim.Id);
            CartViewModel cart = null!;
            if (!string.IsNullOrEmpty(cartString))
            {
                cart = JsonConvert.DeserializeObject<CartViewModel>(cartString!)!;
            }
            if (cartUpdateModel.CartItems != null! && cartUpdateModel.CartItems.Count > 0)
            {
                List<CartItemViewModel> cartItemModels = new List<CartItemViewModel>();
                Dictionary<Guid, int> productIds = new Dictionary<Guid, int>();
                decimal amount = 0;
                foreach (var item in cartUpdateModel.CartItems)
                {
                    productIds.Add(item.ProductId, item.Quantity);
                    amount = item.Price * item.Quantity;
                }
                if (amount != cartUpdateModel.Amount) throw new Exception("Amount invalid");
                try
                {
                    var products = await _productRepository.GetProductByIds(productIds.Keys.ToList(), "None");
                    if (cart == null!)
                    {
                        cart = new CartViewModel
                        {
                            Id = Guid.NewGuid(),
                            Amount = cartUpdateModel.Amount,
                            UserId = claim.Id,
                            User = AutoMapperConfig.Mapper.Map<UserViewModel>(user)
                        };
                    }
                    foreach (var item in products)
                    {
                        var cartItemvm = new CartItemViewModel();

                        cartItemModels.Add(new CartItemViewModel
                        {
                            CartId = cart.Id,
                            Price = item.Price,
                            Id = Guid.NewGuid(),
                            ProductId = item.Id,
                            Product = AutoMapperConfig.Mapper.Map<ProductViewModel>(item),
                            Quantity = productIds[item.Id],
                        });
                    }
                    cart.CartItems = cartItemModels;
                    await _distributedCache.SetStringAsync(claim.Id + "", JsonConvert.SerializeObject(cart));
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                await _distributedCache.RemoveAsync(claim.Id + "");
            }
            return await GetCart();
        }
    }
}
