using Lookeach.Core.ViewModels.Colors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookeach.Application.Queries.Colors
{
    public record GetColorByIdQuery(Guid id):IRequest<ColorViewModel>
    {
    }
}
