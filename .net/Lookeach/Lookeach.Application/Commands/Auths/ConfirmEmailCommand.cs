using Lookeach.Core.ViewModels.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookeach.Application.Commands.Auths
{
    public record ConfirmEmailCommand(string code) : IRequest<UserViewModel>
    {
    }
}
