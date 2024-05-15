using System.Globalization;

namespace Lookeach.Application.Exceptions
{
    public class PayPalException : Exception
    {
        public PayPalException(string msg) : base(msg) { }

        public PayPalException(string msg, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, $"PayPal Exception: {msg}", args)) { }
    }
}
