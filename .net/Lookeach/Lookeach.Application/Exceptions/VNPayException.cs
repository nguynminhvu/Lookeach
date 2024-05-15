using System.Globalization;

namespace Lookeach.Application.Exceptions
{
    public class VNPayException:Exception
    {
        public VNPayException() : base() { }

        public VNPayException(string message) : base(message) { }

        public VNPayException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, $"VNPay Exception: {message}", args)) { }
       
    }
}
