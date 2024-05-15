using Lookeach.Application.AutoMappers;
using Lookeach.Application.DTOs;
using Lookeach.Application.Exceptions;
using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using Lookeach.Core.Enums;
using Lookeach.Core.Repositories;
using Lookeach.Core.UnitOfWorks;
using Lookeach.Core.ViewModels.Orders;
using Lookeach.Core.ViewModels.VNPays;
using Lookeach.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Globalization;
using System.Net;
using System.Text;
namespace Lookeach.Infrastructure.Services
{
    /// <summary>
    /// I do it, because in the future we can scale module payment so easy:
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
        private readonly SortedList<string, string> pParams = new();

        private readonly VNPayDTO _vnpayDTO = new VNPayDTO();
        private readonly IOrderRepository _orderRepository;

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
            _orderRepository = _uow.OrderRepository;
        }

        public async Task<string> GetUrlPayment(Core.Entities.Order order)
        {
            var claim = await GetClaimDTOByAccessToken();

            if (claim == null!)
            {
                throw new UnauthorizedAccessException("Login session expired");
            }

            bool check = false;
            var transactionMasters = await _transactionMasterRepository.GetMany(x => x.OrderId == order.Id).ToListAsync();
            TransactionMaster transactionMaster = null!;
            var paymentMethod = await _paymentMethodRepository.GetMany(x => x.Name == PaymentMethodEnum.VNPay.ToString()).FirstOrDefaultAsync();
            if (paymentMethod == null!) throw new ArgumentNullException("Not found VNPay payment method");
            foreach (var item in transactionMasters)
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

        public async Task<OrderViewModel> HandleQuery(string query)
        {
            var queryString = GetFromQuery(query);
            if (pParams["vnp_ResponseCode"] + "" != "00")
            {
                throw new VNPayException("Not found order");
            }
            var response = Utils.HmacSHA512(_vnpayDTO.HashSetcret, queryString);
            if (!response.Equals(pParams["vnp_SecureHash"] + "", StringComparison.InvariantCultureIgnoreCase))
                throw new VNPayException("Validation VNPay Hash Error");

            var transactionMaster = await _transactionMasterRepository.FirstOrDefaultAsync(x => x.Id == Guid.Parse(pParams["vnp_TxnRef"] + "") && !x.IsCompleted);

            if (transactionMaster == null!)
            {
                throw new VNPayException("Not found order");
            }

            transactionMaster.IsCompleted = true;
            _transactionMasterRepository.Update(transactionMaster);
            await _uow.SaveChangesAsync();

            var vNPayTransaction = new VNPayTransaction
            {
                OrderId = transactionMaster.OrderId,
                Amount = double.Parse(pParams["vnp_Amount"] + ""),
                BankCode = pParams["vnp_BankCode"],
                BankTranNo = pParams["vnp_BankTranNo"],
                CardType = pParams["vnp_CardType"],
                PayDate = DateTime.ParseExact(pParams["vnp_PayDate"] + "", "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                TmnCode = pParams["vnp_TmnCode"],
                TransactionNo = pParams["vnp_TransactionNo"],
                Id = transactionMaster.Id
            };

            var order =
                await _orderRepository.GetOrder(vNPayTransaction.OrderId);

            if (order == null)
                throw new VNPayException("Not found order");

            if (double.TryParse(pParams["vnp_Amount"] + "", out var amount))
            {
                amount /= 100;

                if ((double)order.Amount != amount)
                    throw new VNPayException("Amount invalid");
            }
            await _uow.VNPayTransactionRepository.AddAsync(vNPayTransaction);
            await _uow.SaveChangesAsync();

            return AutoMapperConfig.Mapper.Map<OrderViewModel>(order);
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

        public async Task<OrderViewModel> RefundVNPay(Guid orderId)
        {
            var claim = await GetClaimDTOByAccessToken();
            if (claim == null!)
            {
                throw new UnauthorizedAccessException("Login session expired");
            }
            var transactionMasters = await _transactionMasterRepository.GetMany(x => x.OrderId == orderId && x.IsCompleted).ToListAsync();

            if (transactionMasters == null || transactionMasters.Count == 0)
                throw new VNPayException("Order transaction not found");

            var order = await _orderRepository.GetOrder(orderId);

            if (order == null)
                throw new VNPayException("Order transaction not found");

            foreach (var item in transactionMasters)
            {
                var transVNPay = await _uow.VNPayTransactionRepository.FirstOrDefaultAsync(x => x.Id == item.Id);

                if (transVNPay == null)
                    throw new VNPayException("Order transaction not found");

                var vNPayRefund = new VNPayRefundRequestModel
                {
                    vnp_RequestId = DateTime.Now.Ticks.ToString(),
                    vnp_Version = VNPayLibrary.VERSION,
                    vnp_Command = "refund",
                    vnp_TmnCode = _vnpayDTO.TmnCode,
                    vnp_TransactionType = "02",
                    vnp_TxnRef = transVNPay.Id + "",
                    vnp_Amount = Convert.ToInt64(transVNPay.Amount) * 100,
                    vnp_TransactionNo = transVNPay.TransactionNo,
                    vnp_TransactionDate = transVNPay.PayDate.ToString("yyyyMMddHHmmss"),
                    vnp_CreateBy = claim.Id + "",
                    vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    vnp_IpAddr = GetIPAddress(),
                    vnp_OrderInfo = "Refund transaction with orderId: " + transVNPay.OrderId,
                    vnp_SecureHash = ""
                };

                var data = vNPayRefund.vnp_RequestId + "|" + vNPayRefund.vnp_Version + "|" + vNPayRefund.vnp_Command + "|" +
                           _vnpayDTO.TmnCode + "|" + vNPayRefund.vnp_TransactionType + "|" + vNPayRefund.vnp_TxnRef + "|" +
                           vNPayRefund.vnp_Amount + "|" + vNPayRefund.vnp_TransactionNo + "|" +
                           vNPayRefund.vnp_TransactionDate + "|" + vNPayRefund.vnp_CreateBy + "|" + vNPayRefund.vnp_CreateDate +
                           "|" + vNPayRefund.vnp_IpAddr + "|" + vNPayRefund.vnp_OrderInfo;

                vNPayRefund.vnp_SecureHash = Utils.HmacSHA512(_vnpayDTO.HashSetcret, data);

                var jsonData = JsonConvert.SerializeObject(vNPayRefund);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(_vnpayDTO.RfApi)!;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                var strData = "";
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    strData = streamReader.ReadToEnd();
                }

                var vNPayRefundResponse = JsonConvert.DeserializeObject<VNPayRefundResponseModel>(strData)!;

                if (vNPayRefundResponse.vnp_ResponseCode == "00")
                {
                    var vNPayTransactionRefund = new VNPayTransactionRefund
                    {
                        Id = transVNPay.Id,
                        Amount = vNPayRefundResponse.vnp_Amount,
                        BankCode = vNPayRefundResponse.vnp_BankCode,
                        PayDate = DateTime.ParseExact(vNPayRefundResponse.vnp_PayDate, "yyyyMMddHHmmss",
                            CultureInfo.InvariantCulture),
                        ResponseId = vNPayRefundResponse.vnp_ResponseId,
                        TmnCode = vNPayRefundResponse.vnp_TmnCode,
                        OrderId = transVNPay.Id,
                        TransactionNo = vNPayRefundResponse.vnp_TransactionNo,
                        TxnRef = vNPayRefundResponse.vnp_TxnRef
                    };

                    await _uow.VNPayTransactionRefundRepository.AddAsync(vNPayTransactionRefund);
                    var rs = await _uow.SaveChangesAsync();
                    if (rs <= 0)
                    {
                        throw new Exception("Can't process this request refund");
                    }
                }
                else
                {
                    throw new Exception("Response VNPay fail");
                }
            }
            return AutoMapperConfig.Mapper.Map<OrderViewModel>(order);
        }

        public async Task<List<VNPayTransaction>> GetVNPayTransactions(Guid? id = null, DateTime? dateFrom = null, DateTime? dateTo = null, int? pageIndex = null, int? pageSize = null)
        => await _vnpayTransactionRepository.GetVNPayTransactions(id, dateFrom, dateTo, pageIndex, pageSize);
    }
}
