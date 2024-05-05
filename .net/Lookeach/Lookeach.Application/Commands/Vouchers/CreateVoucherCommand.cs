using Lookeach.Core.ViewModels.Vouchers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookeach.Application.Commands.Vouchers
{
    public record CreateVoucherCommand(VoucherCreateModel VoucherCreateModel) : IRequest<VoucherViewModel>
    {
    }
}
