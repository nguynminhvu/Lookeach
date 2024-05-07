using Lookeach.Core.ViewModels.PaymentMethods;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookeach.Application.Commands.PaymentMethods
{
    public record CreatePaymentMethodCommand(string Name):IRequest<PaymentMethodViewModel>
    {
    }
}
