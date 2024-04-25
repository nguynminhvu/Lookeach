using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Lookeach.Core.Common.Base
{
    public class BaseEntity<KT>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public KT Id { get; set; }
    }
}
