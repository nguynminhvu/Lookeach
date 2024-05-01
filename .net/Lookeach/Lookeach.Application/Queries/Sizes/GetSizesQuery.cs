using Lookeach.Core.ViewModels.Sizes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookeach.Application.Queries.Sizes
{
    public record GetSizesQuery(string? name = null!, int? pageIndex = null!, int? pageSize = null!) : IRequest<List<SizeViewModel>>
    {
    }
}
