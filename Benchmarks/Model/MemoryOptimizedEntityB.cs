using System.ComponentModel.DataAnnotations.Schema;

namespace Benchmarks.Model
{
    [Table(nameof(MemoryOptimizedEntityB))]
    public class MemoryOptimizedEntityB
    {
        public short Id { get; set; }
        public string MyProperty { get; set; }
    }
}
