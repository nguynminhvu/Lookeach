using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Lookeach.Core.Common.Base
{
    public class Audit<T, KT> : IAudit<T, KT>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public KT Id { get; set; }
        public KT CreatedBy { get; set; }
        public T CreatedOn { get; set; }
        public KT ModifiedBy { get; set; }
        public T ModifiedOn { get; set; }
    }
}
