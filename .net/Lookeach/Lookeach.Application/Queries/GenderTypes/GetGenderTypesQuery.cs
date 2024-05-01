using Lookeach.Core.ViewModels.GenderTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookeach.Application.Queries.GenderTypes
{
    public record GetGenderTypesQuery(string? name=null!, int? pageIndex=null!, int? pageSize=null!):IRequest<List<GenderTypeViewModel>>
    {
    }
}
