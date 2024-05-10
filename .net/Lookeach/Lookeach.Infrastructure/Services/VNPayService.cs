using ArtworkSharing.Core.ViewModels.VNPAYS;
using Lookeach.Application.Helpers;
using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using Lookeach.Core.Enums;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Lookeach.Core.ViewModels.VNPays;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Globalization;
using System.Net;
using System.Text;

namespace Lookeach.Infrastructure.Services
{
    /// <summary>
    /// I do it, because in the future we can scale module payment so fucking easy:
    /// 1 Order - N PaymentMethod, 1 PaymentMethod - 1 Transaction
    /// Id of other payment method(VNPayTransaction table,..) is Id TransactionMaster, 2 record of order in table TransactionMaster can have many payment method
    /// </summary>
    public class VNPayService : TokenService, IVNPayService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDistributedCache _distributedCache;
        private readonly ITransactionMasterRepository _transactionMasterRepository;
        private readonly IVNPayTransactionRepository _vnpayTransactionRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly VNPayDTO _vnpayDTO = new VNPayDTO();
        public VNPayService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer) : base(configuration, httpContextAccessor, connectionMultiplexer, distributedCache)
        {
            _uow = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _distributedCache = distributedCache;
            _transactionMasterRepository = _uow.TransactionMasterRepository;
            _vnpayTransactionRepository = _uow.VNPayTransactionRepository;
            _paymentMethodRepository = _uow.PaymentMethodRepository;
            _configuration.GetSection(nameof(VNPayDTO)).Bind(_vnpayDTO);
        }

        public async Task<string> GetUrlPayment(Core.Entities.Order order)
        {
            var claim = await GetClaimDTOByAccessToken();

            if (claim == null!)
            {
                throw new UnauthorizedAccessException("Login session expired");
            }

            bool check = false;
            var transactionTransfers = await _transactionMasterRepository.GetMany(x => x.OrderId == order.Id).ToListAsync();
            TransactionMaster transactionMaster=null!;
            var paymentMethod = await _paymentMethodRepository.GetMany(x => x.Name == PaymentMethodEnum.VNPay.ToString()).FirstOrDefaultAsync();
            if (paymentMethod == null!) throw new ArgumentNullException("Not found VNPay payment method");
            foreach (var item in transactionTransfers)
            {
                if (!item.IsCompleted && item.PaymentMethodId == paymentMethod.Id)
                {
                    transactionMaster = item;
                    check = true;
                }
            }

            if (!check)
            {
                transactionMaster = TransactionMaster.Create(Guid.NewGuid(), order.Id, paymentMethod.Id, false, claim.Id);
                await _transactionMasterRepository.AddAsync(transactionMaster);
                await _uow.SaveChangesAsync();
            }

            var vnpay = new VNPayLibrary();

            vnpay.AddRequestData("vnp_Version", VNPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", _vnpayDTO.TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString());
            vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedOn.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", GetIPAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Artwork Sharing Payment");
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", _vnpayDTO.ReturnUrl);
            vnpay.AddRequestData("vnp_TxnRef", transactionMaster.Id.ToString());
            return vnpay.CreateRequestUrl(_vnpayDTO.Url, _vnpayDTO.HashSetcret);
        }

        public async Task<VNPayViewModel> HandleQuery(string query)
        {
            var queryString = GetFromQuery(query);
            if (pParams["vnp_ResponseCode"] + "" != "00")
            {
                return new VNPayResponseModel
                {
                    TransactionViewModel = null!,
                    IpnResponseViewModel = new IpnResponseViewModel
                    {
                        Message = ResponseMessage.TransactionNotFound,
                        RspCode = "01"
                    }
                };
            }
            var response = Utils.HmacSHA512(Vnpay.HashSetcret, queryString);
            if (!response.Equals(pParams["vnp_SecureHash"] + "", StringComparison.InvariantCultureIgnoreCase))
                return new VNPayResponseModel
                {
                    TransactionViewModel = null!,
                    IpnResponseViewModel = new IpnResponseViewModel
                    {
                        Message = ResponseMessage.ValidateHashError,
                        RspCode = "99"
                    }
                };

            var transactionTransfer = await _uow.VNPayTransactionTransferRepository.FirstOrDefaultAsync(x => x.Id == Guid.Parse(pParams["vnp_TxnRef"] + "") && !x.IsCompleted);

            if (transactionTransfer == null!)
            {
                return new VNPayResponseModel
                {
                    TransactionViewModel = null!,
                    IpnResponseViewModel = new IpnResponseViewModel
                    {
                        Message = ResponseMessage.TransactionNotFound,
                        RspCode = "01"
                    }
                };
            }

            transactionTransfer.IsCompleted = true;
            _uow.VNPayTransactionTransferRepository.UpdateVNPayTransactionTransfer(transactionTransfer);
            await _uow.SaveChangesAsync();
            var vNPayTransaction = new VNPayTransaction
            {
                TransactionId = transactionTransfer.TransactionId,
                Amount = double.Parse(pParams["vnp_Amount"] + ""),
                BankCode = pParams["vnp_BankCode"],
                BankTranNo = pParams["vnp_BankTranNo"],
                CardType = pParams["vnp_CardType"],
                PayDate = DateTime.ParseExact(pParams["vnp_PayDate"] + "", "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                TmnCode = pParams["vnp_TmnCode"],
                TransactionNo = pParams["vnp_TransactionNo"],
                Id = transactionTransfer.Id
            };

            var transaction =
                await _uow.TransactionRepository.FirstOrDefaultAsync(x => x.Id == vNPayTransaction.TransactionId);

            if (transaction == null)

                return new VNPayResponseModel
                {
                    TransactionViewModel = null!,
                    IpnResponseViewModel = new IpnResponseViewModel
                    {
                        Message = ResponseMessage.TransactionNotFound,
                        RspCode = "01"
                    }
                };

            if (double.TryParse(pParams["vnp_Amount"] + "", out var amount))
            {
                amount /= 100;

                if (transaction.TotalBill != amount)
                    return new VNPayResponseModel
                    {
                        TransactionViewModel = null!,
                        IpnResponseViewModel = new IpnResponseViewModel
                        {
                            Message = ResponseMessage.AmountNotValid,
                            RspCode = "06"
                        }
                    };
            }
            await _uow.VNPayTransactionRepository.AddAsync(vNPayTransaction);
            await _uow.SaveChangesAsync();
            return new VNPayResponseModel
            {
                TransactionViewModel = AutoMapperConfiguration.Mapper.Map<TransactionViewModel>(transaction),
                IpnResponseViewModel = new IpnResponseViewModel
                {
                    Message = ResponseMessage.Success,
                    RspCode = "00"
                }
            };
        }
        private string GetIPAddress()
        {
            return Dns.GetHostByName(Dns.GetHostName()).AddressList[1] + "";
        }
        private string GetFromQuery(string query)
        {
            var str = new StringBuilder();
            if (pParams.Count > 0) pParams.Clear();
            query = query.Substring(1);
            var strSplit = query.Split('&');
            foreach (var item in strSplit)
                if (!string.IsNullOrEmpty(item) && item.StartsWith("vnp_") && item.Split("=").Length > 1)
                    pParams.Add(item.Split("=")[0], item.Split("=")[1]);
            foreach (var item in pParams)
            {
                if (item.Key.IsNullOrEmpty() || item.Value.IsNullOrEmpty() || item.Key == "vnp_SecureHashType" ||
                    item.Key == "vnp_SecureHash") continue;

                str.Append($"{WebUtility.UrlEncode(item.Key)}={WebUtility.UrlEncode(item.Value)}&");
            }

            str.Remove(str.Length - 1, 1);
            return str.ToString().Replace("%2B", "+"); // fck .net. .net 4.5 + is +, higher version + is %2B 
        }
    }
}
