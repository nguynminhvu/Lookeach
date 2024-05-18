using Lookeach.Core.Common.Base;

namespace Lookeach.Application.ViewModels.GenderTypes
{
    public class GenderTypeUpdateModel : BaseEntity<Guid>
    {
        public string Name { get; set; }
    }
}
