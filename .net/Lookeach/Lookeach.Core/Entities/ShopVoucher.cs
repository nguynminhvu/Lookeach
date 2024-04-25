using Lookeach.Core.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookeach.Core.Entities
{
    public class ShopVoucher : Audit<DateTime, Guid>
    {
        public Guid VoucherId { get; set; }
        public Guid UserId { get; set; }
        public Voucher Voucher { get; set; }
        public User User { get; set; }
    }
}
