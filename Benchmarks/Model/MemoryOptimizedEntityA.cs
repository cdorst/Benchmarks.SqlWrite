using System.ComponentModel.DataAnnotations.Schema;

namespace Benchmarks.Model
{
    [Table(nameof(MemoryOptimizedEntityA))]
    public class MemoryOptimizedEntityA
    {
        public int Id { get; set; }

        public MemoryOptimizedEntityB EntityB { get; set; }
        public short EntityBId { get; set; }

        public MemoryOptimizedEntityC EntityC { get; set; }
        public long EntityCId { get; set; }

        public object ToSqlSaveParameters() => new { EntityBId, EntityCId };
    }
}
