using Lookeach.Application.AutoMappers;
using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Lookeach.Core.ViewModels.PaymentMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Lookeach.Infrastructure.Services
{
    public class PaymentMethodService : TokenService, IPaymentMethodService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public PaymentMethodService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {
            _uow = unitOfWork;
            _configuration = configuration;
            _httpAccessor = httpContextAccessor;
            _paymentMethodRepository = _uow.PaymentMethodRepository;
        }

        public async Task<PaymentMethodViewModel> CreatePaymentMethod(string name)
        {
            var claim = await GetClaimDTOByAccessToken();
            if (claim == null!) throw new Exception("Login session expired");

            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Name is required");
            var paymentMethod = PaymentMethod.Create(name, claim.Id);

            await _paymentMethodRepository.AddAsync(paymentMethod);
            await _uow.SaveChangesAsync();
            return await GetPaymentMethodById(paymentMethod.Id);
        }

        public async Task<PaymentMethodViewModel> GetPaymentMethodById(Guid id)
        {
            return AutoMapperConfig.Mapper.Map<PaymentMethodViewModel>(await _paymentMethodRepository.FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task<List<PaymentMethodViewModel>> GetPaymentMethods(string? name = null)
        {
            if (!string.IsNullOrEmpty(name))
                return AutoMapperConfig.Mapper.Map<List<PaymentMethodViewModel>>(await _paymentMethodRepository.GetMany(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync());

            return AutoMapperConfig.Mapper.Map<List<PaymentMethodViewModel>>(await _paymentMethodRepository.GetAll().ToListAsync());
        }

        public async Task<PaymentMethodViewModel> UpdatePaymentMethod(PaymentMethodUpdateModel paymentMethodUpdateModel)
        {
            var claim = await GetClaimDTOByAccessToken();
            if (claim == null!) throw new UnauthorizedAccessException("Login session expired");

            var paymentMethod = await _paymentMethodRepository.FirstOrDefaultAsync(x => x.Id == paymentMethodUpdateModel.Id);
            if (paymentMethod == null) throw new ArgumentNullException("Not found Payment Method");

            paymentMethod.Update(paymentMethodUpdateModel.Name, claim.Id);

            _paymentMethodRepository.Update(paymentMethod);

            await _uow.SaveChangesAsync();

            return await GetPaymentMethodById(paymentMethod.Id);
        }
    }
}
