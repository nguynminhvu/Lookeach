namespace Lookeach.Core.Common.Base
{
    public interface IAudit<T, KT> : IBaseEntity<KT>
    {
        KT CreatedBy { get;  }
        T CreatedOn { get;  }
        KT ModifiedBy { get;  }
        T ModifiedOn { get;  }
    }
}
