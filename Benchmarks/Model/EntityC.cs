using System.ComponentModel.DataAnnotations.Schema;

namespace Benchmarks.Model
{
    [Table(nameof(EntityC))]
    public class EntityC
    {
        public long Id { get; set; }
        public string MyProperty { get; set; }
    }
}
