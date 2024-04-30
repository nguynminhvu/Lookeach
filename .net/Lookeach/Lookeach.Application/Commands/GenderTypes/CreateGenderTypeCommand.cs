using Lookeach.Core.ViewModels.GenderTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookeach.Application.Commands.GenderTypes
{
    public record CreateGenderTypeCommand(string name):IRequest<GenderTypeViewModel>
    {
    }
}
