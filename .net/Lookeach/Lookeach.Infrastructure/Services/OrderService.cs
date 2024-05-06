using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Lookeach.Core.ViewModels.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Lookeach.Infrastructure.Services
{
	public class OrderService : TokenService, IOrderService
	{
		private readonly IHttpContextAccessor _httpContext;
		private readonly IVoucherRepository _voucherRepository;
		private readonly IConfiguration _configuration;
		private readonly IUnitOfWork _uow;
		public OrderService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
		{
			_uow = unitOfWork;
			_configuration = configuration;
			_httpContext = httpContextAccessor;
			_voucherRepository = _uow.VoucherRepository;
		}
		public async Task<OrderViewModel> CreateOrder(OrderCreateModel orderCreateModel)
		{
			var claim = await GetClaimDTOByAccessToken();
			if (claim == null!) throw new UnauthorizedAccessException();
			List<Voucher> vouchers = null!;
			if (orderCreateModel.VoucherIds != null && orderCreateModel.VoucherIds.Count > 0)
			{
				vouchers = new List<Voucher>();
				vouchers = await _voucherRepository.GetMany(x => orderCreateModel.VoucherIds.Contains(x.Id)).ToListAsync();
			}
			Guid orderId = Guid.NewGuid();
			List<OrderItem> orderItems = new List<OrderItem>();
			foreach (var item in orderCreateModel.OrderItems)
			{
				orderItems.Add(OrderItem.Create(orderId, item.ProductId, item.Price, item.Quantity, item.ProductVersion, claim.Id));
			}
			var order = Order.Create(claim.Id, orderCreateModel.Amount, orderCreateModel.OrderItems, orderCreateModel.PaymentMethodId, orderCreateModel.Address, orderCreateModel.Phone, claim.Id, orderCreateModel.Email, vouchers);
		}

		public Task<OrderViewModel> GetOrderById(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<List<OrderViewModel>> GetOrders(DateTime? dateFrom = null, DateTime? dateTo = null)
		{
			throw new NotImplementedException();
		}

		public Task<List<OrderViewModel>> GetOrdersByUserId(Guid id, DateTime? dateFrom = null, DateTime? dateTo = null)
		{
			throw new NotImplementedException();
		}

		public Task<OrderViewModel> UpdateOrder(OrderUpdateModel orderUpdateModel)
		{
			throw new NotImplementedException();
		}
	}
}
