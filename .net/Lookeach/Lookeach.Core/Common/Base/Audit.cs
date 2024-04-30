using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Lookeach.Core.Common.Base
{
    public class Audit<T, KT> : IAudit<T, KT>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public KT Id { get; protected set; }
        public KT CreatedBy { get; protected set; }
        public T CreatedOn { get; protected set; }
        public KT ModifiedBy { get; protected set; }
        public T ModifiedOn { get; protected set; }
    }
}
