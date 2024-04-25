using Lookeach.Core.Entities;
using Lookeach.Core.Repositories;
using Lookeach.Infrastructure.Data;
using Lookeach.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookeach.Infrastructure.Repositories
{
    public class ShopVoucherRepository : Repository<ShopVoucher>, IShopVoucherRepository
    {
        public ShopVoucherRepository(LookeachContext appDbContext) : base(appDbContext)
        {
        }
    }
}
