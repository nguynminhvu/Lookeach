namespace Lookeach.Core.Common.Base
{
    public interface IBaseEntity<KT>
    {
        KT Id { get; }
    }
}
