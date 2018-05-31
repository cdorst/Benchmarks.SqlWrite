using System.ComponentModel.DataAnnotations.Schema;

namespace Benchmarks.Model
{
    [Table(nameof(EntityB))]
    public class EntityB
    {
        public short Id { get; set; }
        public string MyProperty { get; set; }
    }
}
