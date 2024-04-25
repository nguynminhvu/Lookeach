using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Lookeach.Core.Common.Base
{
    public class Audit<T, KT> : IAudit<T, KT>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public KT Id { get; set; }
        public KT CreatedBy { get => CreatedBy; set => CreatedBy = value; }
        public T CreatedOn { get => CreatedOn; set => CreatedOn = value; }
        public KT ModifiedBy { get => ModifiedBy; set => ModifiedBy = value; }

        private T _modifiedOn;
        public T ModifiedOn
        {
            get
            {
                if (typeof(T) == typeof(DateTime))
                {
                    return (T)(object)DateTime.Now;
                }
                return _modifiedOn;

            }
            set => _modifiedOn = value;
        }
    }
}
