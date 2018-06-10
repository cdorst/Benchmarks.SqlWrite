using System.ComponentModel.DataAnnotations.Schema;

namespace Benchmarks.Model
{
    [Table(nameof(MemoryOptimizedEntityC))]
    public class MemoryOptimizedEntityC
    {
        public long Id { get; set; }
        public string MyProperty { get; set; }
    }
}
