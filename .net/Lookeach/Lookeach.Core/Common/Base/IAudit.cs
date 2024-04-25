namespace Lookeach.Core.Common.Base
{
    public interface IAudit<T, KT> : IBaseEntity<KT>
    {
        KT CreatedBy { get; set; }
        T CreatedOn { get; set; }
        KT ModifiedBy { get; set; }
        T ModifiedOn { get; set; }
    }
}
