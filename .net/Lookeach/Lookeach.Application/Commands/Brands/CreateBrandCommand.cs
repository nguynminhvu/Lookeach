using Lookeach.Core.ViewModels.Brands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookeach.Application.Commands.Brands
{
    public record CreateBrandCommand(BrandCreateModel BrandCreateModel) : IRequest<BrandViewModel>
    {
    }
}
